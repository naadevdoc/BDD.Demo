using ShoppingCart.Services.Model.CatalogueService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Services.Services
{
    public interface ICatalogueServices
    {
        AddPersonaResponse AddPersona(AddPersonaRequest request);
        AddProductResponse AddProduct(AddProductRequest request);
        AddExchangeRateResponse AddExchangeRate(AddExchangeRateRequest request);
    }
}
