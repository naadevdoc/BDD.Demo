using ShoppingCart.Services.Model.CatalogueService;
using ShoppingCart.Services.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Services.Model.OperationsService
{
    abstract public class OperationsPersonaRequest : EntityRequest
    {
        public GetPersonaResponse GetPersonaResponse { get; set; } = new GetPersonaResponse() 
        { 
            HttpCode = HttpStatusCode.NoContent, 
            ErrorMessage = "Persona Request must be initialized with an entity from catalogue"
        };
    }
}
