Feature: Basket
As a Customer
I want to get information of checked products in my cart
So I can know how much money will cost me
Background:
	Given the catalogue has following products
	  | product                 | price | currency | discount |
	  | motion-cam-hero-09-2019 | 10    | EUR      | 10%      |
	  | motion-cam-hero-10-2021 | 10    | EUR      | 0%       |
	  | phone-hero-13-2022      | 1342  | EUR      | 0%       |
	  | phone-hero-12-2019      | 999   | EUR      | 5%       |
	And these personas are registered
	  | Name        | Fidelity discount | Preferred currency |
	  | David       | 0%                | EUR                |
	  | Maria       | 20%               | EUR                |
	  | Paul        | 0%                | USD                |
	  | Oshiro      | 10%               | JPY                |
	  | Jules       | 5%                | JPY                |
	And the exchange rate at the time of operation is as follows
	  | From Currency | To Currency | Rate    |
	  | EUR           | USD         | 1.134   |
	  | USD           | EUR         | 0.881   |
	  | USD           | JPY         | 114.386 |
	  | JPY           | USD         | 0.009   |
	  | JPY           | EUR         | 0.008   |
	  | EUR           | JPY         | 129.737 |


Rule: Persona can check in products without purchasing

Scenario: Delta00->This is a dummy scenario to illustrate architecture
	Given I create a sample answer request
	When I send the request to the service
	Then the content of sample answer response will be 'true'

Scenario: Delta01->A persona can check in a single product
	Given I am David
	And I am having an empty cart
	And I check in a product 'motion-cam-hero-10-2021'
	When I list checked in products
	Then there will be a single product with code 'motion-cam-hero-10-2021'
	And cart total will be 10 EUR

Scenario: Delta02->A persona can check in same product several times
	Given I am David
	And I am having an empty cart
	And I add following products to my cart 2 times
	 | product                 |
	 | motion-cam-hero-10-2021 |	 
	When I list checked in products
	Then following products will be found
	 | product                 | prize |
	 | motion-cam-hero-10-2021 | 10    |
	 | motion-cam-hero-10-2021 | 10    |
	And total cost will be 20 EUR

Scenario: Delta03->A persona can also check in several products
	Given I am David
	And I am having an empty cart
	And I add following products to my cart
	 | product                 |
	 | motion-cam-hero-10-2021 |
	 | phone-hero-13-2022      |
	When I list checked in products
	Then following products will be found
	 | product                 | prize |
	 | motion-cam-hero-10-2021 | 10    |
	 | phone-hero-13-2022      | 1342  |
	And cart total will be 1352 EUR

Scenario: Delta04->A persona can check in a product and pay in a different currency than preferred one
	Given I am Paul
	And I am having an empty cart
	And I check in a product 'motion-cam-hero-10-2021'
	When I list checked in products
	Then there will be a single product with code 'motion-cam-hero-10-2021'
	And cart total will be 11.34 USD
	
Rule: A persona can purchase a previously checked in product

Scenario: Delta05->A persona purchases a checked in product
	Given I am Paul
	And I check in a product 'motion-cam-hero-10-2021'
	When I purchase my product
	Then I will receive a message 'Thank you for your purchase'
	

Scenario: Delta06->A persona cannot purchase when cart is empty
	Given I am Paul
	And I am having an empty cart
	When I purchase my product
	Then I will receive a message 'There are no items to purchase'

Rule: Some products have a discount


Scenario: Delta07->A persona who checks in a discounted product will get a discount
	Given I am David
	And I am having an empty cart
	And I add following products to my cart
	 | product                 |
	 | motion-cam-hero-10-2021 |
	 | motion-cam-hero-09-2019 |
	When I list checked in products
	Then following products will be found
	 | product                 | prize |
	 | motion-cam-hero-10-2021 | 10    |
	 | motion-cam-hero-09-2019 | 9     |
	And cart total will be 19 EUR

Rule: Personas have a fidelity discount and this fidelity discount is different depending on buying currency


Scenario: Delta08->A persona with fidelity discount will get a discount on final price
	Given I am Maria
	And I am having an empty cart
	And I check in a product 'motion-cam-hero-10-2021'
	When I list checked in products
	Then there will be a single product with code 'motion-cam-hero-10-2021'
	And cart total will be 8 EUR
	And cart total will show a discount of 2 EUR


Scenario: Delta09->Product discount does not apply for personas with fidelity discount
	Given I am Maria
	And I am having an empty cart
	And I add following products to my cart
	 | product                 |
	 | motion-cam-hero-10-2021 |
	 | motion-cam-hero-9-2019  |
	When I list checked in products
	Then following products will be found
	 | product                | prize |
	 | motion-cam-hero-9-2019 | 10    |
	 | motion-cam-hero-9-2019 | 10    |
	And cart total will be 16 EUR
	And cart total will show a discount of 4 EUR


Scenario: Delta10->Product discount does not apply on a different currency than local one
	Given I am Paul
	And I am having an empty cart
	And I check in a product 'motion-cam-hero-09-2019'
	When I list checked in products
	Then there will be a single product with code 'motion-cam-hero-09-2019'
	And cart total will be 11.34 USD


Scenario: Delta11->Fidelity discount applies on a different currency than local one
	Given I am Jules
	And I am having an empty cart
	And I check in a product 'motion-cam-hero-09-2019'
	When I list checked in products
	Then there will be a single product with code 'motion-cam-hero-09-2019'
	And cart total will be 10.77 USD


Scenario: Delta12->Fidelity discount is assigned to currency
	Given I am Jules
	When I switch my preferred currency from USD to EUR
	Then my fidelity discount will be 0%


Scenario: Delta13->Fidelity discount is persisted when the persona switches back to previous currency
	Given I am Jules
	And I switched my preferred currency from USD to EUR
	When I siwtch my preferred currency from EUR to USD
	Then my fidelity discount will be 5%


Scenario: Delta14->A persona who buys more than 2000 EUR in a single buy upgrades fidelity discount by 1%
	Given I am Paul
	And I add following products to my cart
	 | product            |
	 | phone-hero-13-2022 |
	 | phone-hero-13-2022 |
	When I purchase these products
	Then I will have following messages in my inbox
	 | Message                                                 |
	 | Thank you for your purchase                             |
	 | Congratulations. Now you have a fidelity discount of 1% |


Scenario: Delta15->Maximum fidelity discount for a persona is 20%
	Given I am Maria
	And I add following products to my cart
	 | product            |
	 | phone-hero-13-2022 |
	 | phone-hero-13-2022 |
	When I purchase these products
	Then I will have following messages in my inbox
	 | Message                       |
	 | Thank you for your purchase   |
	 | Your fidelity discount is 20% |