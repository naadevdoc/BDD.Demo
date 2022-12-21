using ShoppingCart.Services.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Services.Model.OperationsService
{
    abstract public class PersonaResponse: EntityResponse
    {
        public Persona? Persona { get; set; }
    }
}
