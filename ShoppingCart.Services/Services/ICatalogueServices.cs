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
        UpsertPersonaResponse UpsertPersona(UpsertPersonaRequest request);
        UpsertProductResponse UpsertProduct(UpsertProductRequest request);
        UpsertExchangeRateResponse UpsertExchangeRate(UpsertExchangeRateRequest request);
        GetPersonaResponse GetPersonaByName(GetPersonaRequest request);
        GetProductResponse GetProductByName(GetProductRequest request);
        GetExchangeRateResponse GetExchangeRate(GetExchangeRateRequest request);
    }
}
