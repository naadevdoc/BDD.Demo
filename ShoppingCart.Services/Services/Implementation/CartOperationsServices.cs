﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ShoppingCart.Services.Model.CatalogueService;
using ShoppingCart.Services.Model.Entities;
using ShoppingCart.Services.Model.Extensions;
using ShoppingCart.Services.Model.OperationsService;
using ShoppingCart.Services.Model.OperationsService.Extensions;

namespace ShoppingCart.Services.Services.Implementation
{
    internal class CartOperationsServices : ICartOperationsServices
    {
        public TransformPriceForPersonResponse TranformPriceForPerson(TransformPriceForPersonRequest request)
        {
            var response = request.ValidateRequest(new TransformPriceForPersonResponse());
            var catalogueService = ServiceFactory.GetA<ICatalogueServices>();
            var getPersonaRequest = new GetPersonaRequest
            {
                Persona = new Persona 
                { 
                    Name = request.PersonaName
                }
            };
            var getPersonaResponse = catalogueService.GetPersonaByName(getPersonaRequest);
            return response.ContinueWhenOk(response => response.CheckGetPersonaResponse(getPersonaResponse))
                           .ContinueWhenOk(response => AlignCurrency(getPersonaResponse.Persona,response))
                           ;
        }
        private TransformPriceForPersonResponse AlignCurrency(Persona? persona, TransformPriceForPersonResponse response) 
        {
            HttpStatusCode httpCode = HttpStatusCode.OK;
            if (persona?.Clone() is Persona newPersona && response.HttpCode == HttpStatusCode.OK)
            {
                var personaProducts = new List<Product>();
                var doNotModifyTheseProducts = newPersona?.CheckedOutProducts != null ? newPersona.CheckedOutProducts.Where(x => x.Currency == newPersona.PreferredCurrency).Select(x => x.Clone() as Product).ToList() : new List<Product?>();
                var modifyTheseProducts = newPersona?.CheckedOutProducts != null ? newPersona.CheckedOutProducts.Where(x => x.Currency != newPersona.PreferredCurrency).Select(x => x.Clone() as Product).ToList() : new List<Product?>();
                httpCode = doNotModifyTheseProducts.Any(x => x == null) || modifyTheseProducts.Any(x => x == null) ? HttpStatusCode.InternalServerError : HttpStatusCode.OK;
                var transformedProducts = modifyTheseProducts.Where(x => x != null)
                                                             .Select(x => x != null ? (Product)x.Clone() : throw new InvalidCastException())
                                                             .Select(x => ProductTransformProduct(x,persona?.PreferredCurrency))
                                                             .Where(x => x !=null)
                                                             .Select(x => x != null ? (Product)x.Clone() : throw new InvalidCastException())
                                                             .ToList();
                var notTransformedProducts = doNotModifyTheseProducts.Where(x => x != null)
                                                                     .Select(x => x != null ? (Product)x.Clone() : throw new InvalidCastException())
                                                                     .ToList();
                personaProducts.AddRange(transformedProducts);
                personaProducts.AddRange(notTransformedProducts);
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
