using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Services.Model.OperationsService
{
    abstract public class PersonaRequest : EntityRequest
    {
        public string PersonaName { get; set; } = string.Empty;
    }
}
