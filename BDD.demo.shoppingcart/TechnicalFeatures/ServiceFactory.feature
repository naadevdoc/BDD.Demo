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
Scenario: Catalogue Services AddProduct
	Given an interface ICatalogueServices
	Then this interface ICatalogueServices contains an operation AddProduct
	And AddProduct operation will have a parameter AddProductRequest inherating EntityRequest
	And AddProduct operation will have an output AddProductResponse inherating EntityResponse

Scenario: Catalogue Services AddPersona
	Given an interface ICatalogueServices
	Then this interface ICatalogueServices contains an operation AddPersona
	And AddPersona operation will have a parameter AddPersonaRequest inherating EntityRequest
	And AddPersona operation will have an output AddPersonaResponse inherating EntityResponse
	
Scenario: Catalogue Services AddExchangeRate
	Given an interface ICatalogueServices
	Then this interface ICatalogueServices contains an operation AddExchangeRate
	And AddExchangeRate operation will have a parameter AddExchangeRateRequest inherating EntityRequest
	And AddExchangeRate operation will have an output AddExchangeRateResponse inherating EntityResponse
	