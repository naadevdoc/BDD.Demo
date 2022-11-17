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
	