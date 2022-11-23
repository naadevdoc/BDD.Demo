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
        public static Persona Clone(this Persona persona)
        {
            return new Persona
            {
                CheckedOutProducts = persona.CheckedOutProducts.Select(p => p.Clone()).ToList(),
                FidelityDiscount = persona.FidelityDiscount,
                Name = persona.Name,
                PreferredCurrency = persona.PreferredCurrency
            };
        }
    }
}
