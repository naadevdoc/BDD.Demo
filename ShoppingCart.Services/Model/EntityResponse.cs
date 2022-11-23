using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Services.Model
{
    public abstract class EntityResponse
    {
        public HttpStatusCode HttpCode { get; set; } = HttpStatusCode.OK;
        public string ErrorMessage { get; set; } = string.Empty;
    }
}
