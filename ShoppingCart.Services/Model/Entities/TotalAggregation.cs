using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Services.Model.Entities
{
    public class TotalAggregation
    {
        public double Total { get; set; }
        public CurrencyType Currency { get; set; }

    }
}
