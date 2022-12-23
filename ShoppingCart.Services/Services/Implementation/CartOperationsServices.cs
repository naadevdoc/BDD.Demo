using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Ninject.Activation;
using ShoppingCart.Services.Model.CatalogueService;
using ShoppingCart.Services.Model.Entities;
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
            return ExecutePurchaseAndSetMessage(response);
        } 

        public TransformPriceForPersonResponse TranformPriceForPerson(TransformPriceForPersonRequest request)
        {
            var response = request.InitializeResponse(new TransformPriceForPersonResponse());
            return response.ContinueWhenOk(response => AlignCurrency(response))
                           ;
        }
        private CommitPurchaseResponse ExecutePurchaseAndSetMessage(CommitPurchaseResponse response)
        {
            response.PurchaseMessage = response.HttpCode == HttpStatusCode.OK ? "Thank you for your purchase" :
                          response.HttpCode == HttpStatusCode.NoContent ? "There are no items to purchase" :
                          string.Empty;
            return response;
        }

        private TransformPriceForPersonResponse AlignCurrency(TransformPriceForPersonResponse response) 
        {
            HttpStatusCode httpCode = HttpStatusCode.OK;
            Persona? persona = response.Persona;
            if (persona?.Clone() is Persona newPersona && response.HttpCode == HttpStatusCode.OK)
            {
                var personaProducts = new List<Product>();
                var doNotModifyTheseProducts = newPersona?.CheckedOutProducts != null ? newPersona.CheckedOutProducts.Where(x => x.Currency == newPersona.PreferredCurrency).Select(x => x.Clone() as Product).ToList() : new List<Product?>();
                var currencyToChangeProducts = newPersona?.CheckedOutProducts != null ? newPersona.CheckedOutProducts.Where(x => x.Currency != newPersona.PreferredCurrency).Select(x => x.Clone() as Product).ToList() : new List<Product?>();
                httpCode = doNotModifyTheseProducts.Any(x => x == null) || currencyToChangeProducts.Any(x => x == null) ? HttpStatusCode.InternalServerError : HttpStatusCode.OK;
                var unifiedCurrencyProducts = currencyToChangeProducts.Where(x => x != null)
                                                             .Select(x => x != null ? (Product)x.Clone() : throw new InvalidCastException())
                                                             .Select(x => ProductTransformProduct(x,persona?.PreferredCurrency))
                                                             .Where(x => x !=null)
                                                             .Select(x => x != null ? (Product)x.Clone() : throw new InvalidCastException())
                                                             .ToList();
                var notTransformedProducts = doNotModifyTheseProducts.Where(x => x != null)
                                                                     .Select(x => x != null ? (Product)x.Clone() : throw new InvalidCastException())
                                                                     .ToList();
                personaProducts.AddRange(unifiedCurrencyProducts);
                personaProducts.AddRange(notTransformedProducts);
                personaProducts.ForEach(x => x.DiscountedPrice = persona.FidelityDiscount == 0 ? x.Price * (0 + x.Discount) : x.DiscountedPrice);
                personaProducts.ForEach(x => x.Price = persona.FidelityDiscount == 0 ? x.Price * (double)(1 - x.Discount) : x.Price);
                //personaProducts.ForEach(x => x.DiscountedPrice += x.Price * (0 + persona.FidelityDiscount));
                //personaProducts.ForEach(x => x.Price *= (double)(1-persona.FidelityDiscount));
                persona.CheckedOutProducts = persona?.CheckedOutProducts != null ? personaProducts : throw new InvalidCastException();
                response.HttpCode= httpCode;
                response.ErrorMessage = httpCode != HttpStatusCode.OK ? "Something went wrong when transforming currency" : string.Empty;
            }
            return response;
        }
        private Product? ProductTransformProduct(Product product, CurrencyType? toCurrency)
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
