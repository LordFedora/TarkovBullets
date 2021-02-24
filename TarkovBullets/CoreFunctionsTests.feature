Feature: CoreFunctions

Background: 
	Given An error of 1% of a bullet
	And An error of 0.01 bullets

	
	
Scenario:  Wearing Armor
	Given You are shooting 12x70 RIP
	And The target is wearing Zhuk-6a
	And The target is hit in the Thorax
	Then The Bullet should be blocked 100% of the time

Scenario: Not wearing Armor
	Given You are shooting 9x18 mm PM PSV
	And The target is unarmored
	And The target is hit in the Thorax
	Then The target should take 69 damage
	And The Bullet should be blocked 0% of the time


Scenario: Wearing Helmet
	Given You are shooting 12x70 5.25mm Buckshot
	And The target is wearing Vulkan-5
	And The target is hit in the Head
	Then The Bullet should be blocked 100% of the time

Scenario: Pen is same as armor class
	Given You are shooting <Bullet>
	And the target is wearing <Armor>
	And The target is hit in the Thorax
	And Running the simulation 1000000 times
	Then The Bullet should be unblocked <Percent>% of the time

	Examples: 
	| Bullet          | Armor                     | Percent |
	| 9x19 mm Pst gzh | PACA Soft Armor           | 88.59   |
	| 9x19 mm AP 6.3  | Zhuk-3 Press armor        | 87.88   |
	| 4.6x30mm FMJ SX | Highcom Trooper TFO armor | 87.18   |
	| 9x39 mm 7N9 SPP | BNTI Korund-VM armor      | 86.48   |

