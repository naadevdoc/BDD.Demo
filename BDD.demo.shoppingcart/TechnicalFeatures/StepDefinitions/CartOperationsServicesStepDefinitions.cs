using ShoppingCart.Services;
using ShoppingCart.Services.Model.CatalogueService;
using ShoppingCart.Services.Model.Entities;
using ShoppingCart.Services.Model.OperationsService;
using ShoppingCart.Services.Services;
using System;
using TechTalk.SpecFlow;

namespace BDD.demo.shoppingcart.TechnicalFeatures.StepDefinitions
{
    [Binding]
    public class CartOperationsServicesStepDefinitions
    {
        ScenarioContext _context;
        public CartOperationsServicesStepDefinitions(ScenarioContext context)
        {
            _context = context;
        }
        [Given(@"a TransformPriceForPersonRequest with an empty product name")]
        public void GivenATransformPriceForPersonRequestWithAnEmptyProductName()
        {
            var getPersonaRequest = new GetPersonaRequest
            {
                Persona = new Persona
                {
                    Name = "boo"
                }
            };
            var getPersonaResponse = ServiceFactory.GetA<ICatalogueServices>().GetPersonaByName(getPersonaRequest);
            var request = new TransformPriceForPersonRequest
            {
                GetPersonaResponse = getPersonaResponse,
            };
            _context.Add(ConstantsStepDefinitions.RequestContextKey, request);
        }



        [Given(@"a TransformPriceForPersonRequest with an empty person name")]
        public void GivenATransformPriceForPersonRequestWithAnEmptyPersonName()
        {
            var request = new TransformPriceForPersonRequest();
            _context.Add(ConstantsStepDefinitions.RequestContextKey, request);
        }
        [Given(@"a TransformPriceForPersonRequest for (.*)")]
        public void GivenATransformPriceForPersonRequestForCarl(string name)
        {
            var getPersonaRequest = new GetPersonaRequest
            {
                Persona = new Persona
                {
                    Name = name
                }
            };
            var getPersonaResponse = ServiceFactory.GetA<ICatalogueServices>().GetPersonaByName(getPersonaRequest);
            var request = new TransformPriceForPersonRequest
            {
                GetPersonaResponse = getPersonaResponse,
            };
            _context.Add(ConstantsStepDefinitions.RequestContextKey, request);
        }
        [Given(@"a CommitPurchase with an empty person name")]
        public void GivenACommitPurchaseWithAnEmptyPersonName()
        {
            var request = new CommitPurchaseRequest();
            _context.Add(ConstantsStepDefinitions.RequestContextKey, request);
        }
        [Given(@"a CommitPurchaseRequest for (.*)")]
        public void GivenACommitPurchaseRequestFor(string name)
        {
            var getPersonaRequest = new GetPersonaRequest
            {
                Persona = new Persona
                {
                    Name = name
                }
            };
            var getPersonaResponse = ServiceFactory.GetA<ICatalogueServices>().GetPersonaByName(getPersonaRequest);
            var request = new CommitPurchaseRequest
            {
                GetPersonaResponse = getPersonaResponse
            };
            _context.Add(ConstantsStepDefinitions.RequestContextKey, request);
        }

        [When(@"operation TranformPriceForPerson is invoked in ICartOperationsServices")]
        public void WhenOperationTranformPriceForPersonIsInvokedInICartOperationsServices()
        {
            var service = ServiceFactory.GetA<ICartOperationsServices>();
            var request = _context.Get<TransformPriceForPersonRequest>(ConstantsStepDefinitions.RequestContextKey);
            var response = service.TranformPriceForPerson(request);
            _context.Add(ConstantsStepDefinitions.ResponseContextKey, response);
        }

        [When(@"operation CommitPurchase is invoked in ICartOperationsServices")]
        public void WhenOperationCommitPurchaseIsInvokedInICartOperationsServices()
        {
            var service = ServiceFactory.GetA<ICartOperationsServices>();
            var request = _context.Get<CommitPurchaseRequest>(ConstantsStepDefinitions.RequestContextKey);
            var response = service.CommitPurchase(request);
            _context.Add(ConstantsStepDefinitions.ResponseContextKey, response);
        }
    }
}
