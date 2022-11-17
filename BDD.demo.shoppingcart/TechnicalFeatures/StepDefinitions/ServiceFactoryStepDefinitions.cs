using ShoppingCart.Services;
using ShoppingCart.Services.Model;
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

        [Given(@"an interface ISampleService")]
        public void GivenAnInterfaceISampleService()
        {
            _scenarioContext.Add(ServiceContextKey, ServiceFactory.GetA<ISampleService>());
        }

        [Given(@"this interface ISampleService contains an operation (.*)")]
        public void GivenThisInterfaceISampleServiceContainsAnOperationGetTrue(string operationName)
        {
            var sampleServiceType = typeof(ISampleService);
            Assert.NotNull(sampleServiceType.GetMethods().FirstOrDefault(x => x.Name.Contains(operationName,StringComparison.InvariantCultureIgnoreCase)));
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
    }
}
