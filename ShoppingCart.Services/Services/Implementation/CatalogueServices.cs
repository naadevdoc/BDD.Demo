using ShoppingCart.Services.Model.CatalogueService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Services.Services.Implementation
{
    internal class CatalogueServices : ICatalogueServices
    {
        public AddProductResponse AddProduct(AddProductRequest request) => new AddProductResponse();
        public AddPersonaResponse AddPersona(AddPersonaRequest request) => new AddPersonaResponse();
        public AddExchangeRateResponse AddExchangeRate(AddExchangeRateRequest request) => new AddExchangeRateResponse();
    }
}
