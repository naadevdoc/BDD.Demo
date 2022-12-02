using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Services.Model.Entities.Extensions
{
    public static class PersonaExtensions
    {
        public static TotalAggregation GetTotal(this Persona persona)
        {
            var total = 0.0;
            var currency = CurrencyType.EUR;
            persona.CheckedOutProducts.ForEach(product => total += product.Price);
            return new TotalAggregation { Total = total, Currency = currency };
        }
    }
}
