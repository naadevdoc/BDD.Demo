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
        public static _EntityResponse SetHttpCode<_EntityResponse>(this _EntityResponse response) 
            where _EntityResponse : EntityResponse
        {
            response.HttpCode = string.IsNullOrEmpty(response.ErrorMessage) ? HttpStatusCode.OK : HttpStatusCode.BadRequest;
            return response;
        }
        public static _EntityResponse SetHttpCodeWithError<_EntityResponse>(this _EntityResponse response, HttpStatusCode httpStatusCode, string errorMessage) 
            where _EntityResponse : EntityResponse
        {
            response.HttpCode = httpStatusCode;
            response.ErrorMessage = errorMessage;
            return response;
        }

        public static _EntityResponse ContinueWhenOk<_EntityResponse>(this _EntityResponse response, Func<_EntityResponse,_EntityResponse> whatToDo) 
            where _EntityResponse : EntityResponse
        {
            return response.HttpCode == HttpStatusCode.OK ? whatToDo(response) : response;
        }
    }
}
