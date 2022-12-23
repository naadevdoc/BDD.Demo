using ShoppingCart.Services.Model.CatalogueService;
using ShoppingCart.Services.Model.Entities;
using ShoppingCart.Services.Model.Extensions;
using ShoppingCart.Services.Services;

namespace ShoppingCart.Services.Model.OperationsService.Extensions
{
    internal static class OperationsPersonaRequestExtensions
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
            where TPersonaRequest : OperationsPersonaRequest 
            where TPersonaResponse: OperationsPersonaResponse 
        {
            defaultResponse.ErrorMessage = request == null ? "Request must be initialized" :
                                    string.IsNullOrEmpty(request?.GetPersonaResponse?.Persona?.Name) ? "Persona name must be filled" :
                                    string.Empty;

            return defaultResponse.SetHttpCode();
        }

        internal static TPersonaResponse InitializeResponse<TPersonaRequest, TPersonaResponse>(this TPersonaRequest request, TPersonaResponse defaultResponse)
            where TPersonaRequest : OperationsPersonaRequest
            where TPersonaResponse : OperationsPersonaResponse
        {
            var response = request.ValidateRequest(defaultResponse);
            return response.ContinueWhenOk(x => x.CheckGetPersona(request.GetPersonaResponse));
        }
    }
}
