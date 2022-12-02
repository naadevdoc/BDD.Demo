using ShoppingCart.Services.Model.CatalogueService;
using ShoppingCart.Services.Model.OperationsService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Services.Services
{
    public interface ICartOperationsServices
    {
        public TransformPriceForPersonResponse TranformPriceForPerson(TransformPriceForPersonRequest request);
    }
}
