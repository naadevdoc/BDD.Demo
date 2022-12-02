using ShoppingCart.Services;
using ShoppingCart.Services.Model.CatalogueService;
using ShoppingCart.Services.Model.Entities;
using ShoppingCart.Services.Services;
using System;
using System.Diagnostics;
using System.Net;
using TechTalk.SpecFlow;

namespace BDD.demo.shoppingcart.BusinessFeatures.StepDefinitions
{
    [Binding]
    public class BasketStepDefinitions
    {
        private readonly ScenarioContext _scenarioContext;
        public BasketStepDefinitions(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }
        [Given(@"the catalogue has following products")]
        public void GivenTheCatalogueHasFollowingProducts(Table table)
        {
            //  | name | price | currency | discount |
            var listOfRequests = table.Rows.Select(row => new UpsertProductRequest
            {
                Product = new Product
                {
                    Name = row["product"],
                    Price = double.Parse(row["price"]),
                    Currency = Enum.Parse<CurrencyType>(row["currency"]),
                    Discount = double.Parse(row["discount"].Replace("%", "")) / 100
                }
            });
            var service = ServiceFactory.GetA<ICatalogueServices>();
            var listOfResponses = listOfRequests.Select(productRequest => service.UpsertProduct(productRequest));
            listOfResponses.ToList().ForEach(response => Assert.True(response.HttpCode >= HttpStatusCode.OK, response.ErrorMessage));
        }

        [Given(@"these personas are registered")]
        public void GivenThesePersonasAreRegistered(Table table)
        {
            //| Name        | Fidelity discount | Preferred currency |
            var listOfRequests = table.Rows.Select(row => new UpsertPersonaRequest
            {
                Persona = new Persona
                {
                    Name = row["Name"],
                    PreferredCurrency = Enum.Parse<CurrencyType>(row["Preferred currency"]),
                    FidelityDiscount = double.Parse(row["Fidelity discount"].Replace("%", "")) / 100
                }
            });
            var service = ServiceFactory.GetA<ICatalogueServices>();
            var listOfResponses = listOfRequests.Select(personaRequest => service.UpsertPersona(personaRequest));
            listOfResponses.ToList().ForEach(response => Assert.True(response.HttpCode >= HttpStatusCode.OK, response.ErrorMessage));
        }

        [Given(@"the exchange rate at the time of operation is as follows")]
        public void GivenTheExchangeRateAtTheTimeOfOperationIsAsFollows(Table table)
        {
            //| From Currency | To Currency | Rate    |
            var listOfRequests = table.Rows.Select(row => new UpsertExchangeRateRequest
            {
                ExchangeRate = new ExchangeRate
                {
                    FromCurrency = Enum.Parse<CurrencyType>(row["From Currency"]),
                    ToCurrency = Enum.Parse<CurrencyType>(row["To Currency"]),
                    Rate = double.Parse(row["Rate"])
                }
            });
            var service = ServiceFactory.GetA<ICatalogueServices>();
            var listOfResponses = listOfRequests.Select(request => service.UpsertExchangeRate(request));
            listOfResponses.ToList().ForEach(response => Assert.True(response.HttpCode >= HttpStatusCode.OK, response.ErrorMessage));
        }
        [Given(@"I have signed in as (.*)")]
        public void GivenIHaveSignedInAsDavid(string name)
        {
            var personaResponse = ServiceFactory.GetA<ICatalogueServices>().GetPersonaByName(new GetPersonaRequest { Persona = new Persona { Name = name } });
            Assert.True(personaResponse.HttpCode >= HttpStatusCode.OK && personaResponse.HttpCode <= HttpStatusCode.OK, personaResponse.ErrorMessage);
            Assert.True(personaResponse.Persona != null, $"Persona with name {name} has not been found in catalogue");
            Assert.True(personaResponse.Persona.Name == name);
            _scenarioContext.Add(ConstantsStepDefinitions.GetPersonaResponseKey, personaResponse);
        }


        private Persona? GetPersonaFromGetPersonaResponse()
        {
            var personaResponse = _scenarioContext.Get<GetPersonaResponse>(ConstantsStepDefinitions.GetPersonaResponseKey);
            Assert.True(personaResponse != null, $"{typeof(GetPersonaResponse).Name} is not filled yet.");
            Assert.True(!string.IsNullOrEmpty(personaResponse?.Persona?.Name), "Something went wrong obtaining personaResponse");
            return personaResponse?.Persona;
        }

        [Given(@"I am having an empty cart")]
        public void GivenIAmHavingAnEmptyCart()
        {
            var persona = GetPersonaFromGetPersonaResponse();
            var checkedOutProducts = persona?.CheckedOutProducts != null ? persona.CheckedOutProducts : throw new InvalidOperationException("CheckedOutProducts not initialized");
            checkedOutProducts.Clear();
            var request = new UpsertPersonaRequest { Persona = persona };
            var response = ServiceFactory.GetA<ICatalogueServices>().UpsertPersona(request);
            Assert.True(response.HttpCode >= HttpStatusCode.OK && response.HttpCode <= HttpStatusCode.Accepted, response.ErrorMessage);
        }
        [Given(@"I check in a product '([^']*)'")]
        public void GivenICheckInAProduct(string productName)
        {
            var personaResponse = _scenarioContext.Get<GetPersonaResponse>(ConstantsStepDefinitions.GetPersonaResponseKey);
            var productResponse = ServiceFactory.GetA<ICatalogueServices>().GetProductByName(new GetProductRequest { Product = new Product { Name = productName } });
            Assert.True(productResponse.HttpCode == HttpStatusCode.OK, productResponse.ErrorMessage);
            Assert.True(personaResponse.HttpCode == HttpStatusCode.OK, personaResponse.ErrorMessage);
            Assert.NotNull(productResponse?.Product);
            Assert.NotNull(personaResponse?.Persona);
            personaResponse.Persona.CheckedOutProducts.Add(productResponse.Product);
            ServiceFactory.GetA<ICatalogueServices>().UpsertPersona(new UpsertPersonaRequest { Persona = personaResponse.Persona });
        }
        [When(@"I list checked in products")]
        public void WhenIListCheckedInProducts()
        {
            var personaResponse = _scenarioContext.Get<GetPersonaResponse>(ConstantsStepDefinitions.GetPersonaResponseKey);
            var persona = personaResponse.Persona;
            var service = ServiceFactory.GetA<ICatalogueServices>();
            var getPersonaRequest = new GetPersonaRequest
            {
                Persona = persona != null ? (Persona)persona.Clone() : throw new InvalidOperationException("Persona is null")
            };
            personaResponse = service.GetPersonaByName(getPersonaRequest);
            Assert.NotNull(personaResponse);
            Assert.True(personaResponse.HttpCode == HttpStatusCode.OK, personaResponse.ErrorMessage);
            _scenarioContext.Remove(ConstantsStepDefinitions.GetPersonaResponseKey);
            _scenarioContext.Add(ConstantsStepDefinitions.GetPersonaResponseKey, personaResponse);
        }
        [Then(@"there will be a single product with code '([^']*)'")]
        public void ThenThereWillBeASingleProductWithCode(string productName)
        {
            var personaResponse = _scenarioContext.Get<GetPersonaResponse>(ConstantsStepDefinitions.GetPersonaResponseKey);
            Assert.True(personaResponse != null,"GetPersonaResponse is not in context. Have you invoked GetPersona service?");
            Assert.True(personaResponse?.Persona != null, $"GetPersona has not Persona attached. Info:{personaResponse.HttpCode}, {personaResponse.ErrorMessage}");
            var product = personaResponse?.Persona?.CheckedOutProducts.FirstOrDefault(x => x.Name== productName);
            Assert.True(product != null, $"{productName} has not been found in checked products");
        }

    }
}
