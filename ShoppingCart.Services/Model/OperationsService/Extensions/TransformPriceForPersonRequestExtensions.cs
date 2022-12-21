using ShoppingCart.Services.Model.CatalogueService;
using ShoppingCart.Services.Model.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Services.Model.OperationsService.Extensions
{
    internal static class TransformPriceForPersonRequestExtensions
    {
        internal static TransformPriceForPersonResponse ValidateRequest(this TransformPriceForPersonRequest request, TransformPriceForPersonResponse defaultResponse) 
        {
            defaultResponse.ErrorMessage = request == null ? "Request must be initialized" :
                                    string.IsNullOrEmpty(request?.PersonaName) ? "Persona name must be filled" :
                                    string.Empty;
            return defaultResponse.SetHttpCode();
        }
    }
}
