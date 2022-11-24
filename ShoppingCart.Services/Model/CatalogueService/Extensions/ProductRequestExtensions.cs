using ShoppingCart.Services.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Services.Model.CatalogueService.Extensions
{
    internal static class ProductRequestExtensions 
    {
        public static Z ValidateRequest<T,Z>(this T request, Z defaultResponse) where T : ProductRequest where Z : ProductResponse
        {
            defaultResponse.ErrorMessage = request == null ? "Request must be initialized" :
                                    request?.Product == null ? "Product must be different than null in request" :
                                    string.IsNullOrEmpty(request?.Product?.Name) ? "Product name must be filled" :
                                    string.Empty;
            defaultResponse.HttpCode = string.IsNullOrEmpty(defaultResponse.ErrorMessage) ? HttpStatusCode.OK : HttpStatusCode.BadRequest;
            return defaultResponse;
        }
    }
}
