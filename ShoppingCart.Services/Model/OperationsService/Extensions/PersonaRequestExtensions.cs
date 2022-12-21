using ShoppingCart.Services.Model.CatalogueService;
using ShoppingCart.Services.Model.Entities;
using ShoppingCart.Services.Model.Extensions;
using ShoppingCart.Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Services.Model.OperationsService.Extensions
{
    internal static class PersonaRequestExtensions
    {
        private static GetPersonaResponse GetPersona(string personaName)
        {
            var catalogueService = ServiceFactory.GetA<ICatalogueServices>();
            var getPersonaRequest = new GetPersonaRequest
            {
                Persona = new Persona
                {
                    Name = personaName,
                }
            };
            return catalogueService.GetPersonaByName(getPersonaRequest);
        }
        internal static TPersonaResponse ValidateRequest<TPersonaRequest,TPersonaResponse>(this TPersonaRequest request, TPersonaResponse defaultResponse) 
            where TPersonaRequest : PersonaRequest 
            where TPersonaResponse: PersonaResponse 
        {
            defaultResponse.ErrorMessage = request == null ? "Request must be initialized" :
                                    string.IsNullOrEmpty(request?.PersonaName) ? "Persona name must be filled" :
                                    string.Empty;

            return defaultResponse.SetHttpCode();
        }

        internal static TPersonaResponse InitializeResponse<TPersonaRequest,TPersonaResponse>(this TPersonaRequest request, TPersonaResponse defaultResponse) 
            where TPersonaRequest : PersonaRequest 
            where TPersonaResponse : PersonaResponse
        {
            var response = request.ValidateRequest(defaultResponse);
            return response.ContinueWhenOk(x => x.CheckGetPersonaResponse(GetPersona(request.PersonaName)));
        }
    }
}
