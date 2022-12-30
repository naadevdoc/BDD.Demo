using ShoppingCart.Services.Model.OperationsService;
using ShoppingCart.Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Principal;
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
            total = persona.FidelityDiscount != 0 ? total * (double)(1 - persona.FidelityDiscount) : total;
            total = Math.Round(total, 2);
            return new TotalAggregation { Total = total, Currency = persona.ActiveCurrency };
        }
        internal static TotalAggregation GetTotalDiscount(this Persona persona)
        {
            var total = 0.0;
            persona.CheckedOutProducts.ForEach(product => total += product.Price);
            total = Math.Round(total, 2);
            var totalDiscount = total - persona.GetTotal().Total;
            return new TotalAggregation { Total = totalDiscount, Currency = persona.ActiveCurrency };
        }

        internal static Dictionary<CurrencyType,double> InitializeCurrencies()
        {
            var currency = new Dictionary<CurrencyType,double>();
            var values = Enum.GetValues<CurrencyType>().ToList();
            values.ForEach(x => currency[x] = (double)0.0); 
            return currency;
        }

        internal static Dictionary<CurrencyType, double> CloneCurrencyMap(this Persona persona)
        {
            var currency = new Dictionary<CurrencyType, double>();
            persona.FidelityDiscountMap.ToList().ForEach(x => currency.Add(x.Key, x.Value));
            return currency;
        }
    }
}
