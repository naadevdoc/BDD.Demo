using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Services.Model.Entities
{
    public class Product
    {
        public string Name { get; set; } = string.Empty;
        public double Price { get; set; } = 0.0;
        public CurrencyType Currency { get; set; } = CurrencyType.EUR;
        public double Discount { get; set; } = 0.0;
    }
}
