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
Scenario: TranformPriceForPerson returns error when Person name is not defined
	Given a TransformPriceForPersonRequest with an empty person name
	When operation TranformPriceForPerson is invoked in ICartOperationsServices
	Then the response HttpCode will be BadRequest
	And response Error message will be 'Persona name must be filled'


@CartOperationsServices
@TranformPriceForPerson
Scenario: TranformPriceForPerson returns Empty when persona has no products
	Given these personas are registered
	  | Name        | Fidelity discount | Preferred currency |
	  | David       | 0%                | EUR                |
	And a TransformPriceForPersonRequest for David
	When operation TranformPriceForPerson is invoked in ICartOperationsServices
	Then the response HttpCode will be NoContent
	And response Error message will be 'There are no items in cart'

@CartOperationsServices
@TranformPriceForPerson
Scenario: TranformPriceForPerson returns Empty when persona does not exist
	Given these personas are registered
	  | Name        | Fidelity discount | Preferred currency |
	  | David       | 0%                | EUR                |
	And a TransformPriceForPersonRequest for Carl
	When operation TranformPriceForPerson is invoked in ICartOperationsServices
	Then the response HttpCode will be NotFound
	And response Error message will be 'User not found'