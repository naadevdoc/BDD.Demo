using ShoppingCart.Services;
using ShoppingCart.Services.Model;
using ShoppingCart.Services.Model.CatalogueService;
using ShoppingCart.Services.Model.SampleService;
using ShoppingCart.Services.Services;
using System;
using TechTalk.SpecFlow;

namespace BDD.demo.shoppingcart.TechnicalFeatures.StepDefinitions
{
    [Binding]
    public class ServiceFactoryStepDefinitions
    {
        const string ServiceContextKey = "ServiceContextKey";
        const string ResponseContextKey = "ResponseContextKey";
        private readonly ScenarioContext _scenarioContext;
        public ServiceFactoryStepDefinitions(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }
        [Given(@"a model containing an entity EntityRequest from which all requests will inherate")]
        public void GivenAModelContainingAnEntityEntityRequestFromWhichAllRequestsWillInherate()
        {
            var sampleRequest = new SampleRequest();
            Assert.True(sampleRequest is EntityRequest);
        }

        [Given(@"a model containing an entity EntityResponse from which all responses will inherate")]
        public void GivenAModelContainingAnEntityEntityResponseFromWhichAllResponsesWillInherate()
        {
            var sampleRequest = new SampleResponse();
            Assert.True(sampleRequest is EntityResponse);
        }
        [Given(@"a static Inversion of Control Module")]
        public void GivenAStaticInversionOfControlModule()
        {
            var type = typeof(ServiceFactory);
            Assert.True(type.IsAbstract && type.IsSealed);
        }

        [Given(@"an interface (.*)")]
        public void GivenAnInterfaceISampleService(string interfaceName)
        {
            switch(interfaceName)
            {
                case "ICatalogueServices":
                    _scenarioContext.Add(ServiceContextKey, ServiceFactory.GetA<ICatalogueServices>());
                    break;
                default:
                    _scenarioContext.Add(ServiceContextKey, ServiceFactory.GetA<ISampleService>());
                    break;
            }
        }


        [Given(@"this interface (.*) contains an operation (.*)")]
        [Then(@"this interface (.*) contains an operation (.*)")]
        public void GivenThisInterfaceISampleServiceContainsAnOperationGetTrue(string interfaceName, string operationName)
        {
            Type serviceType;
            switch (interfaceName)
            {
                case "ICatalogueServices":
                    serviceType = typeof(ICatalogueServices);
                    break;
                default:
                    serviceType = typeof(ISampleService);
                    break;
            }
            Assert.NotNull(serviceType.GetMethods().FirstOrDefault(x => x.Name.Contains(operationName,StringComparison.InvariantCultureIgnoreCase)));
        }

        [When(@"I send a request of type SampleRequest inherating EntityRequest to the service")]
        public void WhenISendARequestOfTypeSampleRequestInheratingEntityRequestToTheService()
        {
            var service = _scenarioContext.Get<ISampleService>(ServiceContextKey);
            var request = new SampleRequest();
            Assert.True(request.GetType().IsSubclassOf(typeof(EntityRequest)));
            var response = service.GetTrue(request);
            _scenarioContext.Add(ResponseContextKey, response);
        }

        [Then(@"the response will be of type (.*) which inherates EntityResponse")]
        public void ThenTheResponseWillBeOfTypeSampleResponseWhichInheratesEntityResponse(string responseTypeName)
        {
            var response = _scenarioContext.Get<SampleResponse>(ResponseContextKey);
            var responseType = response.GetType();
            Assert.True(responseType.Name==responseTypeName);
            Assert.True(responseType.IsSubclassOf(typeof(EntityResponse)));
        }


        [Then(@"SampleResponse will have a property Result with value '([^']*)'")]
        public void ThenSampleResponseWillHaveAPropertyResultWithValue(bool theTrueResult)
        {
            var response = _scenarioContext.Get<SampleResponse>(ResponseContextKey);
            Assert.True(response.IsItTrueOrNot == theTrueResult);
        }


        [Then(@"AddProduct operation will have a parameter AddProductRequest inherating EntityRequest")]
        public void ThenAddProductOperationWillHaveAParameterAddProductRequestInheratingEntityRequest()
        {
            var catalogueServices = _scenarioContext.Get<ICatalogueServices>(ServiceContextKey);
            var request = new AddProductRequest();
            var requestType = request.GetType();
            Assert.True(requestType.Name == "AddProductRequest");
            Assert.True(requestType.IsSubclassOf(typeof(EntityRequest)));
            var response = catalogueServices.AddProduct(request);
            _scenarioContext.Add(ResponseContextKey, response);
        }

        [Then(@"AddProduct operation will have an output AddProductResponse inherating EntityResponse")]
        public void ThenAddProductOperationWillHaveAnOutputAddProductResponseInheratingEntityResponse()
        {
            var response = _scenarioContext.Get<AddProductResponse>(ResponseContextKey);
            var responseType = response.GetType();
            Assert.True(responseType.Name == "AddProductResponse");
            Assert.True(responseType.IsSubclassOf(typeof(EntityResponse)));

        }
        [Then(@"AddPersona operation will have a parameter AddPersonaRequest inherating EntityRequest")]
        public void ThenAddPersonaOperationWillHaveAParameterAddPersonaRequestInheratingEntityRequest()
        {
            var catalogueServices = _scenarioContext.Get<ICatalogueServices>(ServiceContextKey);
            var request = new AddPersonaRequest();
            var requestType = request.GetType();
            Assert.True(requestType.Name == "AddPersonaRequest");
            Assert.True(requestType.IsSubclassOf(typeof(EntityRequest)));
            var response = catalogueServices.AddPersona(request);
            _scenarioContext.Add(ResponseContextKey, response);
        }

        [Then(@"AddPersona operation will have an output AddPersonaResponse inherating EntityResponse")]
        public void ThenAddPersonaOperationWillHaveAnOutputAddPersonaResponseInheratingEntityResponse()
        {
            var response = _scenarioContext.Get<AddPersonaResponse>(ResponseContextKey);
            var responseType = response.GetType();
            Assert.True(responseType.Name == "AddPersonaResponse");
            Assert.True(responseType.IsSubclassOf(typeof(EntityResponse)));
        }

        [Then(@"AddExchangeRate operation will have a parameter AddExchangeRateRequest inherating EntityRequest")]
        public void ThenAddExchangeRateOperationWillHaveAParameterAddExchangeRateRequestInheratingEntityRequest()
        {
            var catalogueServices = _scenarioContext.Get<ICatalogueServices>(ServiceContextKey);
            var request = new AddExchangeRateRequest();
            var requestType = request.GetType();
            Assert.True(requestType.Name == "AddExchangeRateRequest");
            Assert.True(requestType.IsSubclassOf(typeof(EntityRequest)));
            var response = catalogueServices.AddExchangeRate(request);
            _scenarioContext.Add(ResponseContextKey, response);
        }

        [Then(@"AddExchangeRate operation will have an output AddExchangeRateResponse inherating EntityResponse")]
        public void ThenAddExchangeRateOperationWillHaveAnOutputAddExchangeRateResponseInheratingEntityResponse()
        {
            var response = _scenarioContext.Get<AddExchangeRateResponse>(ResponseContextKey);
            var responseType = response.GetType();
            Assert.True(responseType.Name == "AddExchangeRateResponse");
            Assert.True(responseType.IsSubclassOf(typeof(EntityResponse)));
        }

    }
}
