using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Services.Model.Entities.Extensions
{
    internal static class ProductExtensions
    {
        public static Product Clone(this Product product)
        {
#pragma warning disable CS8602 // null has no extensions.
            return new Product {
                Currency = product.Currency,
                Discount = product.Discount,
                Name = product.Name,
                Price = product.Price,
            };
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        }
    }
}
