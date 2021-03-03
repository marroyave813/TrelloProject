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

Scenario Outline:  Get board with invalid elements
	Given the user <user>
	When the user wants to get the board <boardId>
	Then a response with id <responseId> and message <errorMessage> shows

Examples: 
| user        | boardId                    | responseId | errorMessage                            |
| "anonymous" | "60358bb95b8996733ff8a580" | 401        | "unauthorized permission requested"     |
| "Mauricio"  | "1111111"                  | 400        | "invalid id"                            |
| "Mauricio"  | "60358cc95b8996733ff8a580" | 404        | "The requested resource was not found." |

Scenario Outline: Add a member
Given the user "Mauricio"
When the user wants to add the member <username> to the board "60358bb95b8996733ff8a580" with type <memberType>
Then the member "<username>" with type "<memberType>"  is part of the board

Examples: 
| username                | memberType | 
| jquser2                 | admin      | 
| omarandresnarvaezortega | normal     | 
| silvanaperezrojas       | admin      | 

Scenario Outline: Add member with invalid elements
Given the user <user>
When the user wants to add the member <username> to the board <boardId> with type <memberType>
Then a response with id <responseId> and message <errorMessage> shows

Examples: 
| user        | responseId | username                | memberType | errorMessage                            | boardId                    |
| "Mauricio"  | 400        | nonexisting2021         | admin      | "member not found"                      | "60358bb95b8996733ff8a580" |
| "Mauricio"  | 400        | omarandresnarvaezortega | superadmin | "invalid value for type"                | "60358bb95b8996733ff8a580" |
| "anonymous" | 401        | silvanaperezrojas       | admin      | "invalid key"                           | "60358bb95b8996733ff8a580" |
| "Mauricio"  | 404        | silvanaperezrojas       | admin      | "The requested resource was not found." | "60358cc95b8996733ff8c580" |

Scenario: Get the members of a board
Given the user "Mauricio"
When the user wants to get the members in the board "60358bb95b8996733ff8a580"
Then the member "silvanaperezrojas" is part of the boards member list

Scenario Outline: Get member with invalid elements
Given the user <user>
When the user wants to get the members in the board "<boardId>"
Then a response with id <responseId> and message <errorMessage> shows

Examples:
| user        | boardId                    | responseId | errorMessage                            |
| "Mauricio"  | 60358bb95b8996733ff8a580aa | 400        | "invalid id"                            |
| "anonymous" | 60358bb95b8996733ff8a580   | 401        | "unauthorized permission requested"     |
| "Mauricio"  | 60358bb95b8996733ff8a581   | 404        | "The requested resource was not found." |