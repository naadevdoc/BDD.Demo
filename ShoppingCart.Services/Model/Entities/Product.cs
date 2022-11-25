using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Services.Model.Entities
{
    public class Product : NamedEntity
    {
        public double Price { get; set; } = 0.0;
        public CurrencyType Currency { get; set; } = CurrencyType.EUR;
        public double Discount { get; set; } = 0.0;

        public override object Clone()
        {
            return new Product
            {
                Currency = Currency,
                Discount = Discount,
                Name = Name,
                Price = Price,
            };
        }
    }
}
