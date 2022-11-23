using ShoppingCart.Services.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Services.Model.CatalogueService
{
    public abstract class PersonaRequest : EntityRequest
    {
        public Persona Persona { get; set; } = new Persona();
    }
}
