Feature: CoreFunction
	

Scenario: Not wearing Armor
	Given You are shooting 9x18 mm PM PSV
	And The target is unarmored
	And The target is hit in the Thorax
	Then The target should take 69 damage
	And The Bullet should be blocked 0% of the time