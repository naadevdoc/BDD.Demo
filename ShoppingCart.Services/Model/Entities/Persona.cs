using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Services.Model.Entities
{
    public class Persona
    {
        public string Name { get; set; } = string.Empty;
        public CurrencyType PreferredCurrency { get; set; } = CurrencyType.EUR;
        public double FidelityDiscount { get; set; } = 0.0;
        public List<Product> CheckedOutProducts { get; set; } = new List<Product>();
    }
}
