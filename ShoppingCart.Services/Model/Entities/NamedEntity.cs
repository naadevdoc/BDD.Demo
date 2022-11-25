using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Services.Model.Entities
{
    public abstract class NamedEntity : ICloneable
    {
        public string Name { get; set; } = string.Empty;

        public abstract object Clone();
    }
}
