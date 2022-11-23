using ShoppingCart.Services.Model.CatalogueService;
using ShoppingCart.Services.Model.CatalogueService.Extensions;
using ShoppingCart.Services.Model.Entities;
using ShoppingCart.Services.Model.Entities.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Services.Services.Implementation
{
    internal class CatalogueServices : ICatalogueServices
    {
        static IList<Persona> Personas = new List<Persona>();
        public UpsertProductResponse UpsertProduct(UpsertProductRequest request) => new UpsertProductResponse();
        public UpsertPersonaResponse UpsertPersona(UpsertPersonaRequest request)
        {
            var response = request.ValidateRequest(new UpsertPersonaResponse());
            if (response.HttpCode == HttpStatusCode.OK)
            {
#pragma warning disable CS8604 //request.ValidateRequest() takes care of ensuring request?.Persona is not null.
                if (Personas.Any(x => x.Name == request?.Persona?.Name))
                {
                    Personas.Remove(Personas.Single(x => x.Name == request?.Persona?.Name));
                }
                Personas.Add(request.Persona.Clone());
#pragma warning restore CS8604
            }
            return response;
        }
        public UpsertExchangeRateResponse UpsertExchangeRate(UpsertExchangeRateRequest request) => new UpsertExchangeRateResponse();

        public GetPersonaResponse GetPersonaByName(GetPersonaRequest request)
        {
            var response = request.ValidateRequest(new GetPersonaResponse());
            if (response.HttpCode == HttpStatusCode.OK)
            {
                var persona = Personas.FirstOrDefault(x => x.Name.Equals(request?.Persona?.Name));
                if (response.HttpCode == HttpStatusCode.OK)
                {
                    if (persona != null)
                    {
                        response.Persona = persona.Clone();
                    }
                    else
                    {
                        response.HttpCode = HttpStatusCode.NotFound;
                        response.ErrorMessage = $"Persona with name {request?.Persona?.Name} is not found";
                    }
                }
            }
            return response;
        }
    }
}
