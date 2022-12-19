
Feature: CatalogueServices
As a software developer,
I will persist data using ICatalogueServices
So I can store and retrieve it among operations


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
	| GetExchangeRate    | GetExchangeRateRequest    | GetExchangeRateResponse    |

Rule: Catalogue Services allow saving and retrieving Exchange Rate
@CatalogueServices
@UpsertExchangeRate
Scenario: UpsertExchangeRate returns error when request is not initialized
	Given an unitialized UpsertExchangeRate
	When operation UpserExchangeRate is invoked in ICatalogueServices
	Then the response HttpCode will be InternalServerError
	And response Error message will be 'UpsertExchangeRate has not been initialized'
@CatalogueServices
@UpsertExchangeRate
Scenario: UpsertExchangeRate returns error when Rate is not defined
	Given an unitialized UpsertExchangeRate
	And Source currency is EUR
	And Destiny currency is USD
	When operation UpserExchangeRate is invoked in ICatalogueServices
	Then the response HttpCode will be InternalServerError
	And response Error message will be 'Rate has not been initialized'
@CatalogueServices
@UpsertExchangeRate
Scenario: UpsertExchangeRate returns error when Source Currency is not filled
	Given an unitialized UpsertExchangeRate
	And Rate 1.0
	And Destiny currency is USD
	When operation UpserExchangeRate is invoked in ICatalogueServices
	Then the response HttpCode will be InternalServerError
	And response Error message will be 'Source Currency has not been initialized'

@CatalogueServices
@UpsertExchangeRate
Scenario: UpsertExchangeRate returns error when Destiny Currency is not filled
	Given an unitialized UpsertExchangeRate
	And Source currency is EUR
	And Rate 1.0
	When operation UpserExchangeRate is invoked in ICatalogueServices
	Then the response HttpCode will be InternalServerError
	And response Error message will be 'Destiny Currency has not been initialized'

@CatalogueServices
@UpsertExchangeRate
Scenario Outline: UpsertProduct returns error when Rate is not possitive number
	Examples: 
	  | From Currency | To Currency | Rate   | HttpCode   | ErrorMessage           |
	  | EUR           | USD         | -0.001 | BadRequest | Rate must be possitive |
	  | USD           | EUR         | 0      | BadRequest | Rate must be possitive |
	  | USD           | EUR         | 0.001  | Ok         |                        |

@CatalogueServices
@GetExchangeRate
Scenario: A GetExchangeRateRequest with unitialized currency Request shall fail
	Given an GetExchangeRateRequest where Source Currency is EUR
	When operation GetExchangeRate is invoked in ICatalogueServices
	Then the response HttpCode will be InternalServerError
	And response Error message will be 'Exchange Rate Request Destiny has not been initialized'
@CatalogueServices
@GetExchangeRate
Scenario: A GetExchangeRateRequest with unitialized destiny currency shall fail
	Given an GetExchangeRateRequest where Destiny Currency is USD
	When operation GetExchangeRate is invoked in ICatalogueServices
	Then the response HttpCode will be InternalServerError
	And response Error message will be 'Exchange Rate Request Source has not been initialized'

@CatalogueServices
@GetExchangeRate
Scenario: A GetExchangeRateRequest on an uninitialized Request shall fail
	Given an unitialized GetExchangeRateRequest
	When operation GetExchangeRate is invoked in ICatalogueServices
	Then the response HttpCode will be InternalServerError
	And response Error message will be 'Exchange Rate Request has not been initialized'
	
@CatalogueServices
@GetExchangeRate
Scenario: A Exchange Rate Request on a non supported currency will return error
	Given the exchange rate at the time of operation is as follows
	  | From Currency | To Currency | Rate    |
	  | EUR           | USD         | 1.134   |
	  | USD           | EUR         | 0.881   |
	And a request to get Exchange Rate from EUR to EUR
	When operation GetExchangeRate is invoked in ICatalogueServices
	Then the response HttpCode will be NotFound
	And response Error message will be 'JPY to EUR is not supported in currency system'


@CatalogueServices
@GetExchangeRate
Scenario: A Exchange Rate Request to get a nominal exchange
	Given the exchange rate at the time of operation is as follows
	  | From Currency | To Currency | Rate    |
	  | EUR           | USD         | 1.134   |
	  | USD           | EUR         | 0.881   |
	And a request to get Exchange Rate from EUR to USD
	When operation GetExchangeRate is invoked in ICatalogueServices
	Then the response HttpCode will be Ok
	And response Rate will be 1.134
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


