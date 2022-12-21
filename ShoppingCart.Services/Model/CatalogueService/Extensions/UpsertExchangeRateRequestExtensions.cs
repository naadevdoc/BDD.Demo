using ShoppingCart.Services.Model.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Services.Model.CatalogueService.Extensions
{
    internal static class UpsertExchangeRateRequestExtensions
    {
        internal static UpsertExchangeRateResponse ValidateRequest(this UpsertExchangeRateRequest request, UpsertExchangeRateResponse defaultResponse)
        {
           var errorMessage = request == null || request?.ExchangeRate == null || request.ExchangeRateIsJustCreated() ? "UpsertExchangeRate has not been initialized" :
                        request?.ExchangeRate?.Rate == null ? "Rate has not been initialized" :
                        request?.ExchangeRate?.ToCurrency == null ? "Destiny Currency has not been initialized" :
                        request?.ExchangeRate?.FromCurrency == null ? "Source Currency has not been initialized":
                        request?.ExchangeRate?.Rate <= 0 ? "Rate must be possitive":
                        string.Empty;
            return defaultResponse.SetHttpCodeWithError(string.IsNullOrWhiteSpace(errorMessage) ? HttpStatusCode.OK : HttpStatusCode.InternalServerError, errorMessage);
        }

        private static bool ExchangeRateIsJustCreated(this UpsertExchangeRateRequest request)
        {
            bool justCreated = request?.ExchangeRate != null;
            justCreated &= request?.ExchangeRate.Rate == null;
            justCreated &= request?.ExchangeRate.ToCurrency == null;
            justCreated &= request?.ExchangeRate.FromCurrency == null;
            return justCreated;
        }
    }
}
