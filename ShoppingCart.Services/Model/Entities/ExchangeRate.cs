using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Services.Model.Entities
{
    public class ExchangeRate
    {
        public CurrencyType? FromCurrency { get; set; }
        public CurrencyType? ToCurrency { get; set; }
        public double? Rate { get; set; }
        public ExchangeRate Clone() => new ExchangeRate
        {
            FromCurrency = FromCurrency,
            ToCurrency = ToCurrency,
            Rate = Rate
        };
    }
}
