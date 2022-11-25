using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Services.Model.Extensions
{
    internal static class EntityResponseExtensions
    {
        public static T SetHttpCode<T>(this T response) where T : EntityResponse
        {
            response.HttpCode = string.IsNullOrEmpty(response.ErrorMessage) ? HttpStatusCode.OK : HttpStatusCode.BadRequest;
            return response;
        }
        public static T SetHttpCodeWithError<T>(this T response, HttpStatusCode httpStatusCode, string errorMessage) where T : EntityResponse
        {
            response.HttpCode = httpStatusCode;
            response.ErrorMessage = errorMessage;
            return response;
        }

        public static T ContinueWhenOk<T>(this T response, Func<T,T> whatToDo) 
            where T : EntityResponse
        {
            return response.HttpCode == HttpStatusCode.OK ? whatToDo(response) : response;
        }
    }
}
