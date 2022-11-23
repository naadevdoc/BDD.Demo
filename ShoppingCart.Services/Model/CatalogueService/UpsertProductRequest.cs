using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoppingCart.Services.Model.Entities;

namespace ShoppingCart.Services.Model.CatalogueService
{
    public class UpsertProductRequest : EntityRequest
    {
        public Product Product { get; set; } = new Product();
    }
}
