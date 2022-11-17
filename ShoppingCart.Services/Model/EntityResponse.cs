using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Services.Model
{
    public abstract class EntityResponse
    {
        public bool IsItTrueOrNot { get; set; } = true;
    }
}
