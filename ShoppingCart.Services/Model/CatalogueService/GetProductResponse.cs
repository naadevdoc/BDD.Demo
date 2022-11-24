using ShoppingCart.Services.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Services.Model.CatalogueService
{
    public class GetProductResponse : ProductResponse
    {
        public Product? Product { get; set; }
    }
}
