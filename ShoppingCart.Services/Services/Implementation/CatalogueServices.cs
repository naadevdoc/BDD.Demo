using Ninject.Activation;
using ShoppingCart.Services.Model;
using ShoppingCart.Services.Model.CatalogueService;
using ShoppingCart.Services.Model.CatalogueService.Extensions;
using ShoppingCart.Services.Model.Entities;
using ShoppingCart.Services.Model.Entities.Extensions;
using ShoppingCart.Services.Model.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Services.Services.Implementation
{
    internal class CatalogueServices : ICatalogueServices
    {
        /// <summary>
        /// Personas is the dataset storing Personas
        /// </summary>
        static IList<NamedEntity> Personas = new List<NamedEntity>();
        /// <summary>
        /// Products is the dataset storing Products
        /// </summary>
        static IList<NamedEntity> Products = new List<NamedEntity>();
        /// <summary>
        /// ExchangeRates stores the exchange rate between two currencies including fee and taxes
        /// </summary>
        static IList<ExchangeRate> ExchangeRates= new List<ExchangeRate>();
        /// <summary>
        /// UpsertProduct will execute following operations
        /// 1) Validates if the request is correct
        /// 2) Once data is validated, upserts data in the dataset
        /// </summary>
        /// <param name="request">UpsertProductRequest with an entity Product to be upserted</param>
        /// <returns>UpsertProductResponse with HttpCode.Ok if succeeded, HttpCode.NotFound if not in catalogue and HttpCode.BadRequest if request has not properly created</returns>
        public UpsertProductResponse UpsertProduct(UpsertProductRequest request) => 
            request.ValidateRequest(new UpsertProductResponse())
                   .ContinueWhenOk((response) => request.Product.UpsertIntoRepository(Products, response))
                   ;
        /// <summary>
        /// UpsertPersona will execute following operations
        /// 1) Validates if the request is correct
        /// 2) Once data is validated, upserts data in the dataset
        /// </summary>
        /// <param name="request">UpsertPersonaRequest with an entity Persona to be upserted</param>
        /// <returns>UpsertPersonaResponse with HttpCode.Ok if succeeded and HttpCode.BadRequest if request has not properly created</returns>
        public UpsertPersonaResponse UpsertPersona(UpsertPersonaRequest request) => 
            request.ValidateRequest(new UpsertPersonaResponse())
                   .ContinueWhenOk((response)=> request.Persona.UpsertIntoRepository(Personas, response))
                   ;
        public UpsertExchangeRateResponse UpsertExchangeRate(UpsertExchangeRateRequest request) => 
            request.ValidateRequest(new UpsertExchangeRateResponse())
                   .ContinueWhenOk((response) => request.ExchangeRate.UpsertIntoRepository(ExchangeRates,response))
                   ;
        /// <summary>
        /// Finds a persona if exists by name
        /// </summary>
        /// <param name="request">GetPersonaRequest with a Persona. Only Name is needed. All other properties are ignored</param>
        /// <returns>GetPersonaResponse with stored persona and HttpCode.Ok if found and HttpCode.BadRequest if there is any error in request</returns>
        public GetPersonaResponse GetPersonaByName(GetPersonaRequest request) => 
            request.ValidateRequest(new GetPersonaResponse())
                   .ContinueWhenOk((response) => SearchAndSetResponse(response, request))
                   ;
        /// <summary>
        /// Finds a product if exists from product catalog by name
        /// </summary>
        /// <param name="request">GetPersonaRequest with a Persona. Only Name is needed. All other properties are ignored</param>
        /// <returns>GetProductResponse with product in catalog and Http.Ok if found, HttpCode.NotFound if not in catalogue and HttpCode.BadRequest if there is any error in request</returns>
        public GetProductResponse GetProductByName(GetProductRequest request) => 
            request.ValidateRequest(new GetProductResponse())
                   .ContinueWhenOk((response) => SearchAndSetResponse(response, request))
                   ;
        /// <summary>
        /// Obtains Exchagerate from the repository
        /// </summary>
        /// <param name="request">GetExchangeRateRequest with the From and To currencies</param>
        /// <returns>GetExchangeRateResponse with product in catalog and Http.Ok if found, HttpCode.NotFound if not in catalogue and HttpCode.BadRequest if there is any error in request</returns>
        public GetExchangeRateResponse GetExchangeRate(GetExchangeRateRequest request) =>
            request.ValidateRequest(new GetExchangeRateResponse())
                   .ContinueWhenOk((response) => SearchAndSetResponse(response, request))
                   ;
        /// <summary>
        /// Actions driven to fill GetPersonaResponse. It will include Persona when found or NotFound and an custom error message when not found
        /// </summary>
        /// <param name="response">The response to be filled</param>
        /// <param name="request">A valid request</param>
        /// <returns>Filled response</returns>
        private GetPersonaResponse SearchAndSetResponse(GetPersonaResponse response, GetPersonaRequest request)
        {
            response.Persona = SearchByName<Persona>(Personas, request.GetNameFromNamedEntity());
            return response.Persona == null ? response.SetHttpCodeWithError(HttpStatusCode.NotFound, $"Persona with name {request.GetNameFromNamedEntity()} is not found") : response;
        }
        /// <summary>
        /// Actions driven to fill GetProductResponse. It will include Persona when found or NotFound and an custom error message when not found
        /// </summary>
        /// <param name="response">The response to be filled</param>
        /// <param name="request">A valid request</param>
        /// <returns>Filled response</returns>
        private GetProductResponse SearchAndSetResponse(GetProductResponse response,GetProductRequest request)
        {
            response.Product = SearchByName<Product>(Products, request.GetNameFromNamedEntity());
            return response.Product == null ? response.SetHttpCodeWithError(HttpStatusCode.NotFound, $"Product {request.GetNameFromNamedEntity()} is no longer in catalogue") : response;
        }
        /// <summary>
        /// Obtains a copy of the element of a dataset when found by name
        /// </summary>
        /// <typeparam name="Y">NamedEntity. That is an entity which can be idenfied by Name</typeparam>
        /// <param name="namedEntities">DataSet of named entities</param>
        /// <param name="name">The name to search</param>
        /// <returns>A copy of the entity or null when not found</returns>
        private Y? SearchByName<Y>(IList<NamedEntity> namedEntities, string? name)
            where Y : NamedEntity
        {
            return (Y?)(namedEntities.FirstOrDefault(x => x.Name.Equals(name, StringComparison.Ordinal))?.Clone());
        }

        private GetExchangeRateResponse SearchAndSetResponse(GetExchangeRateResponse response, GetExchangeRateRequest request)
        {
            var exchangeRate = ExchangeRates.FirstOrDefault(x => request?.FromCurrency == x.FromCurrency 
                                                                      &&
                                                                      request?.ToCurrency == x.ToCurrency);
            var errorMessage = exchangeRate == null ? $"{request?.FromCurrency} to {request?.ToCurrency} is not supported in currency system" : string.Empty;
            response.ExchangeRate = exchangeRate;
            return response.SetHttpCodeWithError(exchangeRate == null ? HttpStatusCode.NotFound : HttpStatusCode.OK, errorMessage);
        }

    }
}
