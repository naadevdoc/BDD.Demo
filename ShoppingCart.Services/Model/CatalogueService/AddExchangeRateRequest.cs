using ShoppingCart.Services.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Services.Model.CatalogueService
{
    public class AddExchangeRateRequest : EntityRequest
    {
        public ExchangeRate ExchangeRate { get; set; } = new ExchangeRate();
    }
}
