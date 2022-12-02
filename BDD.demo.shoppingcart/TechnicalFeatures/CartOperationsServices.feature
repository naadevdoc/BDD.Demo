Feature: CartOperationsServices
As a software developer,
I will modify cart entities using ICartOperations
So I can execute ETL operations on catalogue

Rule: CartOperation Services to Extract, Transform and Load data

@CartOperationsServices
Scenario Outline: Cart Operation Services
	Given an interface ICartOperationsServices
	Then this interface ICartOperationsServices contains an operation <Operation>
	And <Operation> operation will have a parameter <RequestName> inherating EntityRequest
	And <Operation> operation will have an output <ResponseName> inherating EntityResponse
Examples: 
	| Operation              | RequestName                    | ResponseName                    |
	| TranformPriceForPerson | TransformPriceForPersonRequest | TransformPriceForPersonResponse |

@CartOperationsServices
@TranformPriceForPerson
Scenario: TranformPriceForPerson returns error when Product name is not defined
	Given a TransformPriceForPersonRequest with an empty product name
	When operation TranformPriceForPerson is invoked in ICartOperationsServices
	Then the response HttpCode will be BadRequest
	And response Error message will be 'Product name must be filled'
	
@CartOperationsServices
@TranformPriceForPerson
Scenario: TranformPriceForPerson returns error when Person name is not defined
	Given a TransformPriceForPersonRequest with an empty person name
	When operation TranformPriceForPerson is invoked in ICartOperationsServices
	Then the response HttpCode will be BadRequest
	And response Error message will be 'Persona name must be filled'