using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Ninject.Activation;
using ShoppingCart.Services.Model.CatalogueService;
using ShoppingCart.Services.Model.Entities;
using ShoppingCart.Services.Model.Entities.Extensions;
using ShoppingCart.Services.Model.Extensions;
using ShoppingCart.Services.Model.OperationsService;
using ShoppingCart.Services.Model.OperationsService.Extensions;

namespace ShoppingCart.Services.Services.Implementation
{
    internal class CartOperationsServices : ICartOperationsServices
    {
        public CommitPurchaseResponse CommitPurchase(CommitPurchaseRequest request)
        {
            var response = request.InitializeResponse(new CommitPurchaseResponse());
            try
            {
                var persona = request?.GetPersonaResponse?.Persona;
                var discount = persona != null ? persona.GetDiscountFromTotal(GetExchangeRateForEUR(persona.ActiveCurrency)) : throw new InvalidOperationException("Commit Purchase Request; Persona is null");
                return ExecutePurchaseAndSetMessage(response, discount);
            }
            catch (Exception ops)
            {
                return response.ContinueWhenOk(x => response.SetHttpCodeWithError(HttpStatusCode.InternalServerError, ops.Message));
            }
        } 

        public TransformPriceForPersonResponse TranformPriceForPerson(TransformPriceForPersonRequest request)
        {
            var response = request.InitializeResponse(new TransformPriceForPersonResponse());
            try
            {
                return response.ContinueWhenOk(response => AlignCurrency(response));
            }
            catch (Exception ops)
            {
                return response.ContinueWhenOk(x => response.SetHttpCodeWithError(HttpStatusCode.InternalServerError, ops.Message));
            }
        }
        private double GetExchangeRateForEUR(CurrencyType currency)
        {
            var rate = 1.0;
            if (currency != CurrencyType.EUR) 
            {
                var exchangeRateRequest = new GetExchangeRateRequest
                {
                    FromCurrency = currency,
                    ToCurrency = CurrencyType.EUR,
                };
                var exchangeRateResponse = ServiceFactory.GetA<ICatalogueServices>().GetExchangeRate(exchangeRateRequest);
                rate = (double)(exchangeRateResponse?.ExchangeRate?.Rate != null ? exchangeRateResponse.ExchangeRate.Rate : throw new InvalidOperationException($"{exchangeRateResponse?.HttpCode} : {exchangeRateResponse?.ErrorMessage}"));
            }
            return rate;
        }
        private CommitPurchaseResponse ExecutePurchaseAndSetMessage(CommitPurchaseResponse response, double fidelityDiscountIncrement)
        {
            response.PurchaseMessages.Add(response.HttpCode == HttpStatusCode.OK ? "Thank you for your purchase" :
                          response.HttpCode == HttpStatusCode.NoContent ? "There are no items to purchase" :
                          string.Empty);
            var persona = response.Persona;
            var additionalPurchaseMessages = new List<string>();
            if (persona?.FidelityDiscount != null && fidelityDiscountIncrement > 0 && persona.FidelityDiscount < 0.2)
            {
                persona.FidelityDiscount += fidelityDiscountIncrement;
                additionalPurchaseMessages.Add($"Congratulations. Now you have a fidelity discount of {persona.FidelityDiscount * 100}%");
                var upsertResponse = ServiceFactory.GetA<ICatalogueServices>().UpsertPersona(new UpsertPersonaRequest { Persona = persona});
                response.HttpCode = upsertResponse.HttpCode;
                response.ErrorMessage = upsertResponse.ErrorMessage; 
            }
            if (persona?.FidelityDiscount != null && persona.FidelityDiscount >= 0.2)
            {
                additionalPurchaseMessages.Add($"Your fidelity discount is {persona.FidelityDiscount*100}%");
            }
            response.PurchaseMessages.AddRange(additionalPurchaseMessages);
            return response;
        }

        private TransformPriceForPersonResponse AlignCurrency(TransformPriceForPersonResponse response) 
        {
            HttpStatusCode httpCode = HttpStatusCode.OK;
            Persona? persona = response.Persona;
            if (persona?.Clone() is Persona newPersona && response.HttpCode == HttpStatusCode.OK)
            {
                var personaProducts = new List<Product>();
                var doNotModifyTheseProducts = newPersona?.CheckedOutProducts != null ? newPersona.CheckedOutProducts.Where(x => x.Currency == newPersona.ActiveCurrency).Select(x => x.Clone() as Product).ToList() : new List<Product?>();
                var currencyToChangeProducts = newPersona?.CheckedOutProducts != null ? newPersona.CheckedOutProducts.Where(x => x.Currency != newPersona.ActiveCurrency).Select(x => x.Clone() as Product).ToList() : new List<Product?>();
                doNotModifyTheseProducts.ForEach(x => { if (x != null) { x.DiscountedPrice = persona.FidelityDiscount == 0 ? x.Price * (0 + x.Discount) : x.DiscountedPrice; } });
                doNotModifyTheseProducts.ForEach(x => { if (x != null) { x.Price = persona.FidelityDiscount == 0 ? x.Price * (double)(1 - x.Discount) : x.Price; } });
                var unifiedCurrencyProducts = currencyToChangeProducts.Where(x => x != null)
                                                             .Select(x => x != null ? (Product)x.Clone() : throw new InvalidCastException())
                                                             .Select(x => ProductTransformation(x,persona?.ActiveCurrency))
                                                             .Where(x => x !=null)
                                                             .Select(x => x != null ? (Product)x.Clone() : throw new InvalidCastException())
                                                             .ToList();
                var notTransformedProducts = doNotModifyTheseProducts.Where(x => x != null)
                                                                     .Select(x => x != null ? (Product)x.Clone() : throw new InvalidCastException())
                                                                     .ToList();
                personaProducts.AddRange(unifiedCurrencyProducts);
                personaProducts.AddRange(notTransformedProducts);
                persona.CheckedOutProducts = persona?.CheckedOutProducts != null ? personaProducts : throw new InvalidCastException();
                response.HttpCode= httpCode;
                response.ErrorMessage = httpCode != HttpStatusCode.OK ? "Something went wrong when transforming currency" : string.Empty;
            }
            return response;
        }
        private Product? ProductTransformation(Product product, CurrencyType? toCurrency)
        {
            Product? returnedProduct = null;
            if (product != null && product?.Price != null && product?.Currency != null && toCurrency != null && product.Clone() is Product newProduct)
            {
                returnedProduct = newProduct;
                var getExchangeRequest = new GetExchangeRateRequest
                {
                    FromCurrency = product.Currency,
                    ToCurrency = toCurrency
                };
                var getExchangeResponse = ServiceFactory.GetA<ICatalogueServices>().GetExchangeRate(getExchangeRequest);
                if (getExchangeResponse != null && getExchangeResponse.HttpCode == HttpStatusCode.OK && getExchangeResponse.ExchangeRate != null)
                {
                    var rate = getExchangeResponse?.ExchangeRate?.Rate ?? 0;
                    returnedProduct.Currency = (CurrencyType)toCurrency;
                    returnedProduct.Price *= rate;
                }
            }
            return returnedProduct;
        }
    }
}
