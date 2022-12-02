using ShoppingCart.Services.Model.Entities.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Services.Model.Entities
{
    public class Persona : NamedEntity
    {
        public CurrencyType PreferredCurrency { get; set; } = CurrencyType.EUR;
        public double FidelityDiscount { get; set; } = 0.0;
        public List<Product> CheckedOutProducts { get; set; } = new List<Product>();
        public TotalAggregation TotalAggregation 
        { 
            get { return this.GetTotal(); }
        }

        public override object Clone()
        {

#pragma warning disable CS8619 // Inserting Where p is Product to ensure all p will be Products.
            return new Persona
            {
                CheckedOutProducts = CheckedOutProducts.Where(p => p as Product != null)
                                                       .Select(p => p.Clone() as Product)
                                                       .ToList(),
                FidelityDiscount = FidelityDiscount,
                Name = Name,
                PreferredCurrency = PreferredCurrency
            };
#pragma warning restore CS8619 // Nullability of reference types in value doesn't match target type.
        }
    }
}
