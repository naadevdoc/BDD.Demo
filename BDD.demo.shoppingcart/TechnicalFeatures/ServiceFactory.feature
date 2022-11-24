Feature: ServiceFactory
As a software developer,
I will invoke my services through a service factory
So I can replace and mock the services

Background: 
	Given a model containing an entity EntityRequest from which all requests will inherate
	And a model containing an entity EntityResponse from which all responses will inherate

	Given a static Inversion of Control Module

Scenario: Services will be granted through Inversion of Control
	Given an interface ISampleService
	And this interface ISampleService contains an operation GetTrue
	When I send a request of type SampleRequest inherating EntityRequest to the service
	Then the response will be of type SampleResponse which inherates EntityResponse
	And SampleResponse will have a property Result with value 'True'


Rule: Catalogue Services to enable populating and reading data for the tests

@CatalogueServices
Scenario Outline: Catalogue Services
	Given an interface ICatalogueServices
	Then this interface ICatalogueServices contains an operation <Operation>
	And <Operation> operation will have a parameter <RequestName> inherating EntityRequest
	And <Operation> operation will have an output <ResponseName> inherating EntityResponse
Examples: 
	| Operation          | RequestName               | ResponseName               |
	| UpsertProduct      | UpsertProductRequest      | UpsertProductResponse      |
	| GetProductByName   | GetProductRequest         | GetProductResponse         |
	| UpsertPersona      | UpsertPersonaRequest      | UpsertPersonaResponse      |
	| GetPersonaByName   | GetPersonaRequest         | GetPersonaResponse         |
	| UpsertExchangeRate | UpsertExchangeRateRequest | UpsertExchangeRateResponse |
Rule: Catalogue Services allow saving and retrieving Products
@CatalogueServices
@UpsertProduct

Scenario: Product default values
	Given an unitialized Product
	Then Price will be 0.0
	And Currency will be EUR
	And discount will be 0%
	
@CatalogueServices
@UpsertProduct
Scenario: UpsertProduct returns error when Name of product is not defined
	Given an unitialized Product
	When operation UpsertProduct is invoked in ICatalogueServices
	Then the response HttpCode will be BadRequest
	And response Error message will be 'Product name must be filled'
	
@CatalogueServices
@UpsertProduct
Scenario: UpsertProduct returns error when UpsertProductRequest exists but Product is null
	Given a Product which has been assigned to null
	When operation UpsertProduct is invoked in ICatalogueServices
	Then the response HttpCode will be BadRequest 
	And response Error message will be 'Product must be different than null in request'
	
@CatalogueServices
@UpsertProduct
Scenario: UpsertProductRequest is null returns an error message
	Given an interface ICatalogueService
	When an operation UpserProduct is invoked with a null request
	Then the response HttpCode will be BadRequest
	And response Error message will be 'Request must be initialized'
	
@CatalogueServices
@UpsertProduct
Scenario: GetProductByName returns error when not found in catalogue
	Given a product name 'electric-mug'
	When operation GetProductByName is invoked in ICatalogueServices
	Then the response HttpCode will be NotFound
	And response Error message will be 'Product electric-mug is no longer in catalogue'

Rule: Catalogue Services allow saving and retrieving Personas

@CatalogueServices
@UpsertPersona
Scenario: Persona default values
	Given an unitialized Persona
	Then Persona preffered currency will be EUR
	And fidelity discount will be 0.0
	
@CatalogueServices
@UpsertPersona
Scenario: UpsertPersona returns error when Name of Persona is not defined in UpsertPersonaRequest
	Given a Persona
	When operation UpsertPersona is invoked in ICatalogueServices
	Then the response HttpCode will be BadRequest 
	And response Error message will be 'Persona Name must be filled'
	
@CatalogueServices
@UpsertPersona
Scenario: UpsertPersona returns error when UpsertPersonaRequest exists but Persona is null
	Given a Persona which has been assigned to null
	When operation UpsertPersona is invoked in ICatalogueServices
	Then the response HttpCode will be BadRequest 
	And response Error message will be 'Persona must be different than null in request'

@CatalogueServices
@UpsertPersona
Scenario: UpsertPersonaRequest is null returns an error message
	Given an interface ICatalogueService
	When an operation UpsertPersona is invoked with a null request
	Then the response HttpCode will be BadRequest
	And response Error message will be 'Request must be initialized'


@CatalogueServices
@UpsertPersona
Scenario: GetPersonaByName returns error when not found in catalogue
	Given a persona called 'Bob'
	When operation GetPersonaByName is invoked in ICatalogueServices
	Then the response HttpCode will be NotFound
	And response Error message will be 'Persona with name Bob is not found'

