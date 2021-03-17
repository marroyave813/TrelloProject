Feature: GetBoards
	Get the boards created by each user

@mytag
Scenario: Get board with id
	Given the user "Mauricio"
	When the user wants to get the board "60358bb95b8996733ff8a580"
	Then the board with "60358bb95b8996733ff8a580" is retrieved

Scenario: Get a board from the user
	Given the user "Mauricio"
	When the user wants to get the owning boards
	And gets one of the boards
	Then the board selected board is retrieved

Scenario: Get board with anonymous user
	Given the user "anonymous"
	When the user wants to get the board "60358bb95b8996733ff8a580"
	Then an unauthorized error with text "unauthorized permission requested" shows

Scenario: Get board with invalid id
	Given the user "Mauricio"
	When the user wants to get the board "1111111"
	Then a request error with text "invalid id" shows

Scenario: Get board with non existing id
	Given the user "Mauricio"
	When the user wants to get the board "60358cc95b8996733ff8a580"
	Then a found error with text "The requested resource was not found." shows