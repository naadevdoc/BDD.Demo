using ShoppingCart.Services.Model.Entities;
using ShoppingCart.Services.Model.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Services.Model.CatalogueService.Extensions
{
    internal static class PersonaRequestExtensions 
    {
        internal static Z ValidateRequest<T,Z>(this T request, Z defaultResponse) where T : PersonaRequest where Z : PersonaResponse
        {
            defaultResponse.ErrorMessage = request == null ? "Request must be initialized" :
                                    request?.Persona == null ? "Persona must be different than null in request" :
                                    string.IsNullOrEmpty(request?.Persona?.Name) ? "Persona Name must be filled" :
                                    string.Empty;            
            return defaultResponse.SetHttpCode();
        }
    }
}
