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
            var listOfRequests = table.Rows.Select(row => new AddProductRequest
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
            var listOfResponses = listOfRequests.Select(productRequest => service.AddProduct(productRequest));
            listOfResponses.ToList().ForEach(response => Assert.True(response.HttpCode >= HttpStatusCode.OK, response.ErrorCode));
        }

        [Given(@"these personas are registered")]
        public void GivenThesePersonasAreRegistered(Table table)
        {
            //| Name        | Fidelity discount | Preferred currency |
            var listOfRequests = table.Rows.Select(row => new AddPersonaRequest
            {
                Persona = new Persona
                {
                    Name = row["Name"],
                    PreferredCurrency = Enum.Parse<CurrencyType>(row["Preferred currency"]),
                    FidelityDiscount = double.Parse(row["Fidelity discount"].Replace("%", "")) / 100
                }
            });
            var service = ServiceFactory.GetA<ICatalogueServices>();
            var listOfResponses = listOfRequests.Select(personaRequest => service.AddPersona(personaRequest));
            listOfResponses.ToList().ForEach(response => Assert.True(response.HttpCode >= HttpStatusCode.OK, response.ErrorCode));
        }

        [Given(@"the exchange rate at the time of operation is as follows")]
        public void GivenTheExchangeRateAtTheTimeOfOperationIsAsFollows(Table table)
        {
            //| From Currency | To Currency | Rate    |
            var listOfRequests = table.Rows.Select(row => new AddExchangeRateRequest
            {
                ExchangeRate = new ExchangeRate
                {
                    FromCurrency = Enum.Parse<CurrencyType>(row["From Currency"]),
                    ToCurrency = Enum.Parse<CurrencyType>(row["To Currency"]),
                    Rate = double.Parse(row["Rate"])
                }
            });
            var service = ServiceFactory.GetA<ICatalogueServices>();
            var listOfResponses = listOfRequests.Select(request => service.AddExchangeRate(request));
            listOfResponses.ToList().ForEach(response => Assert.True(response.HttpCode >= HttpStatusCode.OK, response.ErrorCode));
        }
    }
}
