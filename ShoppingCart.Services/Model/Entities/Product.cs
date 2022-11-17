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
        public double Price { get; set; }
        public CurrencyType Currency { get; set; }
        public double Discount { get; set; }
    }
}
