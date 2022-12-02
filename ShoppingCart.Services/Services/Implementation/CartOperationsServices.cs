using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoppingCart.Services.Model.OperationsService;
using ShoppingCart.Services.Model.OperationsService.Extensions;

namespace ShoppingCart.Services.Services.Implementation
{
    internal class CartOperationsServices : ICartOperationsServices
    {
        public TransformPriceForPersonResponse TranformPriceForPerson(TransformPriceForPersonRequest request)
        {
            var response = new TransformPriceForPersonResponse();
            return request.ValidateRequest(response);
        }

    }
}
