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
        public CurrencyType PreferredCurrency { get; set; }
        public double FidelityDiscount { get; set; }
    }
}
