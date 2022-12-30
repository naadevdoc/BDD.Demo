using ShoppingCart.Services.Model.Entities.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Services.Model.Entities
{
    public class Persona : NamedEntity
    {
        public CurrencyType ActiveCurrency { get; set; } = CurrencyType.EUR;
        public double FidelityDiscount
        {
            get => FidelityDiscountMap[ActiveCurrency];
            set => FidelityDiscountMap[ActiveCurrency] = value;
        }
        public Dictionary<CurrencyType, double> FidelityDiscountMap { get; set; } = PersonaExtensions.InitializeCurrencies();
        public List<Product> CheckedOutProducts { get; set; } = new List<Product>();
        public TotalAggregation TotalAggregation 
        { 
            get { return this.GetTotal(); }
        }

        public TotalAggregation TotalDiscount
        {
            get { return this.GetTotalDiscount(); }
        }

        public override object Clone()
        {
            return new Persona
            {
                CheckedOutProducts = CheckedOutProducts.Select(p =>  (Product)p.Clone())
                                                       .ToList(),
                Name = Name,
                ActiveCurrency = ActiveCurrency,
                FidelityDiscountMap = this.CloneCurrencyMap(),
            };
        }

    }
}
