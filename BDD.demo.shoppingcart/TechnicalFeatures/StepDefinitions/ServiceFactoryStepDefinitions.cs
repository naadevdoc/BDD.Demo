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
    public class ServiceFactoryStepDefinitions
    {
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
            switch (interfaceName)
            {
                case "ICatalogueServices":
                    _scenarioContext.Add(ConstantsStepDefinitions.ServiceContextKey, ServiceFactory.GetA<ICatalogueServices>());
                    break;
                default:
                    _scenarioContext.Add(ConstantsStepDefinitions.ServiceContextKey, ServiceFactory.GetA<ISampleService>());
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
            Assert.NotNull(serviceType.GetMethods().FirstOrDefault(x => x.Name.Contains(operationName, StringComparison.InvariantCultureIgnoreCase)));
        }

        [Then(@"(.*) operation will have a parameter (.*) inherating EntityRequest")]
        public void ThenAddProductOperationWillHaveAParameterAddProductRequestInheratingEntityRequest(string operation, string parameter)
        {
            var catalogueServices = _scenarioContext.Get<ICatalogueServices>(ConstantsStepDefinitions.ServiceContextKey);
            var targetOperation = catalogueServices.GetType().Methods().Where(x => x.Name == operation).ToList().FirstOrDefault();
            Assert.True(targetOperation != null, $"Operation {operation} is not available yet");
            var requestType = targetOperation.GetParameters().FirstOrDefault(x => x.ParameterType.Name == parameter)?.ParameterType;
            Assert.True(requestType != null, $"Parameter {parameter} is not detected as part of the service");
            var requestInstance = Activator.CreateInstance(requestType) as EntityRequest;
            Assert.True(requestInstance != null, $"Request type should be an EntityRequest entity");
        }

        [Then(@"(.*) operation will have an output (.*) inherating EntityResponse")]
        public void ThenAddProductOperationWillHaveAnOutputAddProductResponseInheratingEntityResponse(string operation, string returnedParameter)
        {
            var catalogueServices = _scenarioContext.Get<ICatalogueServices>(ConstantsStepDefinitions.ServiceContextKey);
            var targetOperation = catalogueServices.GetType().Methods().Where(x => x.Name == operation).ToList().FirstOrDefault();
            Assert.True(targetOperation != null, $"Operation {operation} is not available yet");
            var responseType = targetOperation.ReturnType;
            Assert.True(responseType.Name == returnedParameter, $"Returned parameter should {returnedParameter} and it is {responseType.Name}");
            var requestInstance = Activator.CreateInstance(responseType) as EntityResponse;
            Assert.True(requestInstance != null, $"Request type should be an EntityResponse entity");

        }
    }
}
