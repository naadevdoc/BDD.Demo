using ShoppingCart.Services.Model.Extensions;
using ShoppingCart.Services.Model.OperationsService;
using ShoppingCart.Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Services.Model.CatalogueService.Extensions
{
    internal static class PersonaResponseExtensions
    {

        internal static TPersonaResponse TransformProductPrices<TPersonaResponse>(this TPersonaResponse response)
            where TPersonaResponse : OperationsPersonaResponse
        {
            var persona = response.Persona != null ? response.Persona : throw new InvalidOperationException("PersonaResponseExtensions:TransformProductPrices should work with a valid response");
            var transformPricesRequest = new TransformPriceForPersonRequest { GetPersonaResponse = new GetPersonaResponse { Persona = persona} };
            var transformPricesResponse = ServiceFactory.GetA<ICartOperationsServices>().TranformPriceForPerson(transformPricesRequest);
            response.Persona = transformPricesResponse.HttpCode == HttpStatusCode.OK ? transformPricesResponse.Persona : persona;
            return response.SetHttpCodeWithError(transformPricesResponse.HttpCode, transformPricesResponse.ErrorMessage);
        }

    }
}
