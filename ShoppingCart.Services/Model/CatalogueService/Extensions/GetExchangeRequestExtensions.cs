using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ShoppingCart.Services.Model.Extensions;

namespace ShoppingCart.Services.Model.CatalogueService.Extensions
{
    internal static class GetExchangeRequestExtensions
    {
        public static GetExchangeRateResponse ValidateRequest(this GetExchangeRateRequest request, GetExchangeRateResponse defaultResponse)
        {
            var errorMessage = request == null ? "Request must be initialized" :
                                    request.FromCurrency == null && request.ToCurrency == null ? "Exchange Rate Request has not been initialized" :
                                    request.FromCurrency == null ? "Exchange Rate Request Source has not been initialized" :
                                    request.ToCurrency == null ? "Exchange Rate Request Destiny has not been initialized" :                                    
                                    string.Empty;
            var httpCode = request == null || request.FromCurrency == null || request.ToCurrency == null ? HttpStatusCode.InternalServerError : HttpStatusCode.OK;
            return defaultResponse.SetHttpCodeWithError(httpCode,errorMessage);
        }
    }
}
