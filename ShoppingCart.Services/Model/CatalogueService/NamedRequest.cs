using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Services.Model.CatalogueService
{
    public abstract class NamedRequest : EntityRequest
    {
        public abstract string GetNameFromNamedEntity();
    }
}
