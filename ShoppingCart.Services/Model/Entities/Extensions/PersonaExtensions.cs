using ShoppingCart.Services.Model.OperationsService;
using ShoppingCart.Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Services.Model.Entities.Extensions
{
    internal static class PersonaExtensions
    {
        internal static TotalAggregation GetTotal(this Persona persona)
        {
            var total = 0.0; 
            persona.CheckedOutProducts.ForEach(product => total += product.Price);
            return new TotalAggregation { Total = total, Currency = persona.PreferredCurrency };
        }
        internal static TotalAggregation GetTotalDiscount(this Persona persona)
        {
            var totalDiscount = 0.0;
            persona.CheckedOutProducts.ForEach(product => totalDiscount += product.DiscountedPrice);
            return new TotalAggregation { Total = totalDiscount, Currency = persona.PreferredCurrency };
        }
    }
}
