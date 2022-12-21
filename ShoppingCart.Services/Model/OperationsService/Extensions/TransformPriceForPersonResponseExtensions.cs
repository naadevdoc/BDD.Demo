using ShoppingCart.Services.Model.CatalogueService;
using ShoppingCart.Services.Model.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Services.Model.OperationsService.Extensions
{
    public static class TransformPriceForPersonResponseExtensions
    {
        internal static TransformPriceForPersonResponse CheckGetPersonaResponse(this TransformPriceForPersonResponse response, GetPersonaResponse getPersonaResponse)
        {
            var products = getPersonaResponse?.Persona?.CheckedOutProducts;
            var noProducts = products == null || products.Count == 0;
            response.Persona = getPersonaResponse?.Persona;
            response = getPersonaResponse != null && getPersonaResponse.HttpCode == HttpStatusCode.NotFound ? response.SetHttpCodeWithError(HttpStatusCode.NotFound, "User not found") :
                       getPersonaResponse != null && getPersonaResponse.HttpCode != HttpStatusCode.OK ? response.SetHttpCodeWithError(getPersonaResponse.HttpCode, getPersonaResponse.ErrorMessage) :
                       noProducts ? response.SetHttpCodeWithError(HttpStatusCode.NoContent, "There are no items in cart") :
                       response;
            return response;
        }

    }
}
