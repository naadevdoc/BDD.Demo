using Ninject.Activation;
using ShoppingCart.Services;
using ShoppingCart.Services.Model;
using ShoppingCart.Services.Model.CatalogueService;
using ShoppingCart.Services.Model.Entities;
using ShoppingCart.Services.Model.SampleService;
using ShoppingCart.Services.Services;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Mail;
using System.Xml.Linq;
using TechTalk.SpecFlow;

namespace BDD.demo.shoppingcart.TechnicalFeatures.StepDefinitions
{
    [Binding]
    public class CatalogueServicesStepDefinitions
    {
        private readonly ScenarioContext _scenarioContext;
        public CatalogueServicesStepDefinitions(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [Given(@"a persona called '([^']*)'")]
        public void GivenAPersonaCalled(string name)
        {
            var persona = new Persona { Name = name };
            _scenarioContext.Add(ConstantsStepDefinitions.RequestContextKey, new GetPersonaRequest { Persona = persona });
        }
        [Given(@"a product name '(.*)'")]
        public void GivenAProductName(string name)
        {
            var product = new Product { Name = name };
            _scenarioContext.Add(ConstantsStepDefinitions.RequestContextKey, new GetProductRequest { Product = product });
        }


  [When(@"operation GetPersonaByName is invoked in ICatalogueServices")]
        public void WhenOperationGetPersonaIsInvokedInICatalogueServices()
        {
            var request = _scenarioContext.Get<GetPersonaRequest>(ConstantsStepDefinitions.RequestContextKey);
            var service = ServiceFactory.GetA<ICatalogueServices>();
            _scenarioContext.Add(ConstantsStepDefinitions.ResponseContextKey, service.GetPersonaByName(request));
        }
        [When(@"operation GetProductByName is invoked in ICatalogueServices")]
        public void WhenOperationGetProductByNameIsInvokedInICatalogueServices()
        {
            var request = _scenarioContext.Get<GetProductRequest>(ConstantsStepDefinitions.RequestContextKey);
            var service = ServiceFactory.GetA<ICatalogueServices>();
            _scenarioContext.Add(ConstantsStepDefinitions.ResponseContextKey, service.GetProductByName(request));
        }

        [When(@"I send a request of type SampleRequest inherating EntityRequest to the service")]
        public void WhenISendARequestOfTypeSampleRequestInheratingEntityRequestToTheService()
        {
            var service = _scenarioContext.Get<ISampleService>(ConstantsStepDefinitions.ServiceContextKey);
            var request = new SampleRequest();
            Assert.True(request.GetType().IsSubclassOf(typeof(EntityRequest)));
            var response = service.GetTrue(request);
            _scenarioContext.Add(ConstantsStepDefinitions.ResponseContextKey, response);
        }

        [Then(@"the response will be of type (.*) which inherates EntityResponse")]
        public void ThenTheResponseWillBeOfTypeSampleResponseWhichInheratesEntityResponse(string responseTypeName)
        {
            var response = _scenarioContext.Get<SampleResponse>(ConstantsStepDefinitions.ResponseContextKey);
            var responseType = response.GetType();
            Assert.True(responseType.Name==responseTypeName);
            Assert.True(responseType.IsSubclassOf(typeof(EntityResponse)));
        }


        [Then(@"SampleResponse will have a property Result with value '([^']*)'")]
        public void ThenSampleResponseWillHaveAPropertyResultWithValue(bool theTrueResult)
        {
            var response = _scenarioContext.Get<SampleResponse>(ConstantsStepDefinitions.ResponseContextKey);
            Assert.True(response.IsItTrueOrNot == theTrueResult);
        }

        [Given(@"an unitialized Product")]
        public void GivenAnUnitializedProduct()
        {
            _scenarioContext.Add(ConstantsStepDefinitions.RequestContextKey, new UpsertProductRequest());
        }

        [Given(@"a Persona")]
        [Given(@"an unitialized Persona")]
        public void GivenAnPersona()
        {
            _scenarioContext.Add(ConstantsStepDefinitions.RequestContextKey, new UpsertPersonaRequest { Persona = new Persona() });
        }
        [Given(@"a Persona which has been assigned to null")]
        public void GivenAPersonaWhichHasBeenAssignedToNull()
        {
#pragma warning disable CS8625 // Extreme test case.
            _scenarioContext.Add(ConstantsStepDefinitions.RequestContextKey, new UpsertPersonaRequest { Persona = null });
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

        }
        [Given(@"a Product which has been assigned to null")]
        public void GivenAProductWhichHasBeenAssignedToNull()
        {
#pragma warning disable CS8625 // Extreme test case.
            _scenarioContext.Add(ConstantsStepDefinitions.RequestContextKey, new UpsertProductRequest { Product = null });
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

        }

        [When(@"operation UpsertPersona is invoked in ICatalogueServices")]
        public void WhenOperationAddPersonaIsInvoked()
        {
            var request = _scenarioContext.Get<UpsertPersonaRequest>(ConstantsStepDefinitions.RequestContextKey);
            _scenarioContext.Add(ConstantsStepDefinitions.ResponseContextKey, ServiceFactory.GetA<ICatalogueServices>().UpsertPersona(request));
        }
        [When(@"operation UpsertProduct is invoked in ICatalogueServices")]
        public void WhenOperationUpserProductIsInvokedInICatalogueServices()
        {
            var request = _scenarioContext.Get<UpsertProductRequest>(ConstantsStepDefinitions.RequestContextKey);
            _scenarioContext.Add(ConstantsStepDefinitions.ResponseContextKey, ServiceFactory.GetA<ICatalogueServices>().UpsertProduct(request));
        }
        [Then(@"Price will be (.*)")]
        public void ThenPriceWillBe(double price)
        {
            var request = _scenarioContext.Get<UpsertProductRequest>(ConstantsStepDefinitions.RequestContextKey);
            Assert.True(request.Product.Price == price, $"Default price must be {price}");
        }

        [Then(@"Currency will be (.*)")]
        public void ThenCurrencyWillBeEUR(CurrencyType currency)
        {
            var request = _scenarioContext.Get<UpsertProductRequest>(ConstantsStepDefinitions.RequestContextKey);
            Assert.True(request.Product.Currency == currency, $"Default currency must be {currency}");

        }
        [Then(@"discount will be (.*)%")]
        public void ThenDiscountWillBe(int discountPercentage)
        {
            var request = _scenarioContext.Get<UpsertProductRequest>(ConstantsStepDefinitions.RequestContextKey);
            Assert.True(request.Product.Discount == discountPercentage, $"Default discount must be {discountPercentage}");
        }


        [Then(@"Persona preffered currency will be (.*)")]
        public void ThenPersonaPrefferedCurrencyWillBeEUR(CurrencyType currency)
        {
            var request = _scenarioContext.Get<UpsertPersonaRequest>(ConstantsStepDefinitions.RequestContextKey);
            Assert.True(request.Persona.PreferredCurrency == currency, $"Default preferred currency must be {currency}");
        }

        [Then(@"fidelity discount will be 0.0")]
        public void ThenFidelityDiscountWillBe()
        {
            var request = _scenarioContext.Get<UpsertPersonaRequest>(ConstantsStepDefinitions.RequestContextKey);
            Assert.True(request.Persona.FidelityDiscount == 0.0, $"Default fidelity discount must be 0.0");
        }

        [Then(@"the response HttpCode will be (.*)")]
        public void ThenTheResponseHttpCodeWillBe(HttpStatusCode httpCode)
        {
            var response = _scenarioContext.Get<EntityResponse>(ConstantsStepDefinitions.ResponseContextKey);
            Assert.True(response.HttpCode == httpCode);
        }
        [Then(@"response Error message will be '([^']*)'")]
        public void ThenResponseErrorMessageWillBe(string message)
        {
            var response = _scenarioContext.Get<EntityResponse>(ConstantsStepDefinitions.ResponseContextKey);
            Assert.True(response.ErrorMessage == message);
        }

        [When(@"an operation UpsertPersona is invoked with a null request")]
        public void WhenAnOperationUpsertPersonaIsInvokedWithANullRequest()
        {
#pragma warning disable CS8625 // Extreme test case.
            _scenarioContext.Add(ConstantsStepDefinitions.ResponseContextKey, ServiceFactory.GetA<ICatalogueServices>().UpsertPersona(null));
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
        }
        [When(@"an operation UpserProduct is invoked with a null request")]
        public void WhenAnOperationUpserProductIsInvokedWithANullRequest()
        {
#pragma warning disable CS8625 // Extreme test case.
            _scenarioContext.Add(ConstantsStepDefinitions.ResponseContextKey, ServiceFactory.GetA<ICatalogueServices>().UpsertProduct(null));
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
        }

    }
}
