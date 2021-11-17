Feature: Boards
	Interact with Trello boards 

@mytag
Scenario: Get board with id
	Given the user "Mauricio"
	When the user wants to get the board "604ce460ef515f36c3f2bb9a"
	Then the board with "604ce460ef515f36c3f2bb9a" is retrieved


Scenario: Get a board from the user
	Given the user "Mauricio"
	When the user wants to get the owning boards
	And gets one of the boards
	Then the board selected board is retrieved

Scenario: Get board with anonymous user
	Given the user "anonymous2"
	When the user wants to get the board "604ce460ef515f36c3f2bb9a"
	Then an unauthorized error with text "unauthorized permission requested" shows

Scenario: Get board with invalid id
	Given the user "Mauricio"
	When the user wants to get the board "1111111"
	Then a request error with text "invalid id" shows

Scenario: Get board with non existing id
	Given the user "Mauricio"
	When the user wants to get the board "604ce460ef515f36c3f2bb92"
	Then a found error with text "The requested resource was not found." shows


#Update Boards

Scenario Outline: Successfully update an existent board
	Given the user "Silvana"
	When the user wants to update the <name>, <desc>, <prefs/permissionLevel>, <prefs/selfJoin> and <labelNames/yellow> fields in the board "604ce460ef515f36c3f2bb9a"
	Then the board fields are updated with new values

Examples:
| name         | desc         | prefs/permissionLevel | prefs/selfJoin | labelNames/yellow |
| Don't Delete | Update board | private               | true           | TEST1             |


Scenario Outline: Board not updated when invalid fields are sent
	Given the user "Silvana"
	When the user wants to send invalid values to <closed>, <prefs/selfJoin>, <prefs/cardCovers> and <prefs/hideVotes> fields in the board "60358bb95b8996733ff8a580"
	Then a response with id <responseId> and a message <errorMessage> is retrieved

Examples:
| closed    | prefs/selfJoin | prefs/cardCovers | prefs/hideVotes |responseId  | errorMessage                            |
| "test123" |    true        |      true        |    true         | 400        | invalid value for closed                |
|   true    |  test123       |      true        |     true        | 400        | invalid value for prefs/selfJoin        |
|   false   |   true         |     test123      |     false       | 400        | invalid value for prefs/cardCovers      |
|   true    |   false        |       true       |  test123        | 400        | invalid value for prefs/hideVotes       |


#Create Labels on a board

Scenario Outline: Create a label from a board
	Given the user "Silvana"
	When the user creates a label with <name> and <color> in the board "604ce460ef515f36c3f2bb9a"
	Then the user should get a new label with <name> and <color> from the board "604ce460ef515f36c3f2bb9a"

		Examples:
	| name      | color |
	| Label_123 | pink  |
	| Test_1    |  sky  |
	| label_spr | lime  |	
	
Scenario Outline: Create a label with invalid values
	Given the user <user> 
	When the user creates a label with name "test" color <color> in the board <boardId> with invalid values
	Then a response with id <responseId> and a message <errorMessage> is retrieved

	Examples:
	|  user    |          boardId       | color  |responseId|      errorMessage                                   |
	|"anonymus"|604ce460ef515f36c3f2bb9a| pink   |  401     |      invalid key                                    |
    |"Silvana" |	      test123       | red    |  400     |      invalid id                                     |
	|"Silvana" |604ce460ef515f36c3f2bb9a| brown  |  400     |{"message":"invalid value for color","error":"ERROR"}|

	
#Get Labels on a board

Scenario: Get all labels from a board
	Given the user "Silvana"
	When the user wants to get all the labels from the board "604ce460ef515f36c3f2bb9a"
	Then all labels should be retrieved  
	

Scenario Outline: Get labels filtered from the board
	Given the user "Silvana"
	When the user wants to get some labels filetered by <fields> and <limit> from the board "604ce460ef515f36c3f2bb9a"
	Then only the filtered labels should be return in the response

		Examples:
	|fields|limit|
	|label |  1  |
	| name |  3  |
    |color |  5	 |
	

Scenario Outline: Get labels with invalid data
	Given the user <user>
	When the user wants to get a label with name "test" and invalid data to boardId <boardId>, fields <fields> and limit <limit>
	Then a response with id <responseId> and a message <errorMessage> is retrieved
	
	Examples:
	|  user    |         boardId        |fields| limit|responseId| errorMessage    |
	|"anonymus"|604ce460ef515f36c3f2bb9a|label |   3  |  401     | invalid key     |
	|"Silvana" |         Test123        | name |   5  |  400     | invalid id      |

Scenario Outline: Get member with invalid elements
Given the user <user>
When the user wants to get the members in the board "<boardId>"
Then a response with id <responseId> and message <errorMessage> shows

Examples:
| user        | boardId                    | responseId | errorMessage                            |
| "Mauricio"  | 60358bb95b8996733ff8a580aa | 400        | "invalid id"                            |
| "anonymous" | 60358bb95b8996733ff8a580   | 401        | "unauthorized permission requested"     |
| "Mauricio"  | 60358bb95b8996733ff8a581   | 404        | "The requested resource was not found." |


Scenario Outline: Delete a Board
Given the user <user>
When the user wants to get the board "<boardId>"
And delete it with Id "<boardId>" and error message "<ServerMessage>"
Then the system should not show the board with id "<boardId>" anymore and Status code is <statusCodeErrorMessage> and <errorMessage>

Examples:
| user        | boardId                    | statusCode | ServerMessage                          | statusCodeErrorMessage    |  errorMessage                            |
| "JQ"        | 604ce3750c7c104d4b477dd0   |     200    | null                                   |      404                  |  "The requested resource was not found." |  
| "anonymous" | 604ce3750c7c104d4b477dd0   |     401    | "unauthorized permission requested"    |      401                  |  "The requested resource was not found." |


Scenario Outline: Delete a Board already Deleted
Given the user <user>
When the user try to get the board "<boardId>" with error message <errorMessage>
Then the board is already deleted and Status code is <statusCode>

Examples:
| user        | boardId                    | statusCode | errorMessage                             |
| "JQ"        | 604ce3750c7c104d4b477dd0   |     404    |"The requested resource was not found."   |
| "anonymous" | 604ce3750c7c104d4b477dd0   |     401    |"unauthorized permission requested"       |  


Scenario Outline: Delete a Private Board
Given the user <user>
When the user wants to get the private board "<boardId>"
Then the system should not let delete the private board and throw a Status code "<statusCode>"

Examples:
| user        | boardId                    |  statusCode   | 
| "Mauricio"  | 604aac8258824285539dbbea   |  Unauthorized |
| "anonymous" | 604aac8258824285539dbbea   |     401       |


