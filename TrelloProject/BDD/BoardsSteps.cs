using FluentAssertions;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;
using TrelloProject.Model.JsonModel;

namespace TrelloProject.BDD
{
	[Binding]
	public sealed class BoardsSteps
	{
		//User variables
		string key;
		string token;
		string userId;
		string _boardId;
		string _labelsId;
		string _labelsColor;
		string _labelsName;

		Random rdm = new Random();

		//API variables
		IRestClient client = new RestClient();
		IRestRequest request;
		IRestResponse response;

		//API Endpoints
		string getBoards = "https://api.trello.com/1/boards/";
		string getListOfBoards = "https://api.trello.com/1/members/";

        [Given(@"the user ""(.*)""")]
		public void GivenTheUser(string userName)
		{
			//Method to set the keys, tokens and usernames of the current users
			switch (userName)
			{
				case "Mauricio":
					key = "78faad10e6c2a1b55dd05729ab5fad4d";
					token = "c1c25177a8ca251ae153cce02d8bb02969572f1412e9c7fb26d0e23d05ef192c";
					userId = "marroyaveuser";
					break;
				case "Silvana":
					key = "6db2bc5a0880e96440976fd9b770a030";
					token = "371aada84a8a60edf434d721597de68de147cb0a414d63374e49d0f5f698da3b";
					userId = "silvanaperezrojas";
					break;
				default:
					key = "";
					token = "";
					userId = "";
					break;
			}
		}

		[When(@"the user wants to add the member (.*) to the board ""(.*)"" with type (.*)")]
		public void WhenTheUserWantsToAddTheMemberToTheBoardWithType(object userName, string boardId, string memberType)
		{
			//Set the request to add a member to the board
			request = new RestRequest(getBoards + boardId + "/members/" + userName);
			request.AddHeader("Accept", "application/json");
			request.AddQueryParameter("key", key);
			request.AddQueryParameter("token", token);
			request.AddQueryParameter("type", memberType);
			response = client.Put(request);
		}

		[When(@"the user wants to get the board ""(.*)""")]
		public void WhenTheUserWantsToGetTheBoard(string boardId)
		{
			//Set the request to get a board
			request = new RestRequest(getBoards + boardId);
			request.AddHeader("Accept", "application/json");
			request.AddQueryParameter("key", key);
			request.AddQueryParameter("token", token);
			response = client.Get(request);
		}

		[When(@"the user wants to get the owning boards")]
		public void WhenTheUserWantsToGetTheOwningBoards()
		{
			//Set the request to get the boards of a user
			request = new RestRequest(getListOfBoards + userId + "/boards");
			request.AddHeader("Accept", "application/json");
			request.AddQueryParameter("key", key);
			request.AddQueryParameter("token", token);
			response = client.Get(request);
		}

		[When(@"gets one of the boards")]
		public void WhenGetsOneOfTheBoards()
		{
			//Get the boards of the user
			List<BoardList> myBoards = BoardList.Deserialize(response);
			//Get a random id from the list
			_boardId = myBoards[rdm.Next(0, myBoards.Count - 1)].id;

			//Set the request to get a random board
			request = new RestRequest(getBoards + _boardId);
			request.AddHeader("Accept", "application/json");
			request.AddQueryParameter("key", key);
			request.AddQueryParameter("token", token);
			response = client.Get(request);
		}

		[When(@"the user wants to get the members in the board ""(.*)""")]
		public void WhenTheUserWantsToGetTheMembersInTheBoard(string boardId)
		{
			//Set the request to get the members of a board
			request = new RestRequest(getBoards + boardId + "/members/");
			request.AddHeader("Accept", "application/json");
			request.AddQueryParameter("key", key);
			request.AddQueryParameter("token", token);
			response = client.Get(request);
		}


		[Then(@"the board with ""(.*)"" is retrieved")]
		public void ThenTheBoardWithIsRetrieved(string boardId)
		{
			//Check the response code
			response.StatusCode.Should().Be(200);
			//Get the boards
			Board myBoard = Board.Deserialize(response);
			//Check the id of the board
			myBoard.id.Should().Be(boardId);
		}


		[Then(@"the board selected board is retrieved")]
		public void ThenTheBoardSelectedBoardIsRetrieved()
		{
			//Check the response code
			response.StatusCode.Should().Be(200);
			//Get the boards
			Board myBoard = Board.Deserialize(response);
			//Check the id of the board
			myBoard.id.Should().Be(_boardId);
		}

		[Then(@"a response with id (.*) and message ""(.*)"" shows")]
		public void ThenAResponseWithIdAndMessageShows(int responseId, string errorMessage)
		{
			//Check the status code from the response
			response.StatusCode.Should().Be(responseId);
			//Check the error message
			response.Content.Should().Be(errorMessage);
		}

<<<<<<< HEAD
		[Then(@"the member ""(.*)"" with type ""(.*)""  is part of the board")]
		public void ThenTheMemberWithTypeIsPartOfTheBoard(object user, object type)
		{
			bool result = false;
			//Get the members and the memberships from the response
			Member_Membership members = Member_Membership.Deserialize(response);
			//Get the member with the selected user name
			Member selectedMember = members.members.First(x => x.username.Equals(user.ToString()));
			//Get the membership of the selected user name - Username and type should be equal
			members.memberships.Should().Contain(x => x.memberType.Equals(type.ToString()) && x.idMember.Equals(selectedMember.id));
		}

		[Then(@"the member ""(.*)"" is part of the boards member list")]
		public void ThenTheMemberIsPartOfTheBoardsMemberList(string userName)
		{
			//Check the response code
			response.StatusCode.Should().Be(200);
			//Get the members
			List<Member> members = Member.Deserialize(response);
			members.Should().Contain(x => x.username.Equals(userName));
		}
=======
		//-------------------//----------------------------------//--------------------------------//

		[When(@"the user wants to update the (.*), (.*), (.*), (.*) and (.*) fields in the board ""(.*)""")]
		public void WhenTheUserWantsToUpdateTheAndFieldsInTheBoard(string name, string desc, string permissionLevel, string selfJoin, string yellow, string boardId)
		{
		request = new RestRequest(getBoards + boardId);
			request.AddHeader("Accept", "application/json");
			request.AddQueryParameter("key", key);
			request.AddQueryParameter("token", token);
			request.AddQueryParameter("name", name);
			request.AddQueryParameter("desc", desc);
			request.AddQueryParameter("prefs/permissionLevel", permissionLevel);
			request.AddQueryParameter("prefs/selfJoin", selfJoin);
			request.AddQueryParameter("labelNames/yellow", yellow);
		response = client.Put(request);
		}

		[Then(@"the board fields are updated with new values")]
		public void ThenTheBoardFieldsAreUpdatedWithNewValues()
		{
		response.StatusCode.Should().Be(200);
			Board myBoard = Board.Deserialize(response);
			myBoard.name.Should().Be("Don't Delete");
			myBoard.desc.Should().Be("Update board");
			myBoard.prefs.permissionLevel.Should().Be("private");
			myBoard.prefs.selfJoin.Should().Equals(true);
			myBoard.labelNames.yellow.Should().Be("TEST1");
		}

		[When(@"the user wants to send invalid values to (.*), (.*), (.*) and (.*) fields in the board ""(.*)""")]
		public void WhenTheUserWantsToSendInvalidValuesToAndFieldsInTheBoard(string closed, string selfJoin, string cardCovers, string hideVotes, string boardId)
		{
		request = new RestRequest(getBoards + boardId);
			request.AddHeader("Accept", "application/json");
			request.AddQueryParameter("key", key);
			request.AddQueryParameter("token", token);
			request.AddQueryParameter("closed", closed);
			request.AddQueryParameter("prefs"+"/"+"selfJoin", selfJoin);
			request.AddQueryParameter("prefs"+"/"+"cardCovers", cardCovers);
			request.AddQueryParameter("prefs"+"/"+"hideVotes", hideVotes);
		response = client.Put(request);
		}

		[Then(@"a response with id (.*) and a message (.*) is retrieved")]
		public void ThenAResponseWithIdAndAMessageIsRetrieved(int responseId, string errorMessage)
		{
			response.StatusCode.Should().Be(responseId);
			response.Content.Should().Be(errorMessage);

		}


		//-------------------------//----------------------------//-------------------------------------------//

		[When(@"the user creates a label with (.*) and (.*) in the board ""(.*)""")]
		public void WhenTheUserCreatesALabelWithAndInTheBoard(string name, string color, string boardId)
		{
		request = new RestRequest(getBoards + boardId + "/labels");
			request.AddHeader("Accept", "application/json");
			request.AddQueryParameter("key", key);
			request.AddQueryParameter("token", token);
			request.AddQueryParameter("name", name);
			request.AddQueryParameter("color", color);
		response = client.Post(request);
		}

		[Then(@"the user should get a new label with (.*) and (.*) from the board ""(.*)""")]
		public void ThenTheUserShouldGetANewLabelWithAndFromTheBoard(string name, string color, string boardId)
		{
			response.StatusCode.Should().Be(200); 
			Labels mylabels = Labels.Deserialize(response);
			mylabels.id.Should().NotBeNullOrEmpty();
			mylabels.idBoard.Should().Be(boardId);
			mylabels.name.Should().Be(name);
			mylabels.color.Should().Be(color);

		}

		[When(@"the user creates a label with name ""(.*)"" color (.*) in the board (.*) with invalid values")]
		public void WhenTheUserCreatesALabelWithNameColorInTheBoardWithInvalidValues(string name, string color, string boardId)
		{
		request = new RestRequest(getBoards + boardId + "/labels");
			request.AddHeader("Accept", "application/json");
			request.AddQueryParameter("key", key);
			request.AddQueryParameter("token", token);
			request.AddQueryParameter("name", name);
			request.AddQueryParameter("color", color);
		response = client.Post(request);
		}

		//--------------------------------------//----------------------------------//-----------------------------//

		[When(@"the user wants to get all the labels from the board ""(.*)""")]
		public void WhenTheUserWantsToGetAllTheLabelsFromTheBoard(string boardId)
		{
		request = new RestRequest(getBoards + boardId + "/labels");
			request.AddHeader("Accept", "application/json");
			request.AddQueryParameter("key", key);
			request.AddQueryParameter("token", token);
		response = client.Get(request);
		}

		[Then(@"all labels should be retrieved")]
		public void ThenAllLabelsShouldBeRetrieved()
		{
		response.StatusCode.Should().Be(200);
			List <LabelsList> mylabels = LabelsList.Deserialize(response);
			for (int i = 0; i < mylabels.Count; i++)
			{
				Console.WriteLine(mylabels[i].id, mylabels[i].idBoard, mylabels[i].name, mylabels[i].color);
			}
		}

		[When(@"the user wants to get some labels filetered by (.*) and (.*) from the board ""(.*)""")]
		public void WhenTheUserWantsToGetSomeLabelsFileteredByAndFromTheBoard(string fields, string limit, string boardId)
		{
			request = new RestRequest(getBoards + boardId + "/labels");
				request.AddHeader("Accept", "application/json");
				request.AddQueryParameter("key", key);
				request.AddQueryParameter("token", token);
				request.AddQueryParameter("fields", fields);
				request.AddQueryParameter("limit", limit);
			response = client.Get(request);
		}

		[Then(@"only the filtered labels should be return in the response")]
		public void ThenOnlyTheFilteredLabelsShouldBeReturnInTheResponse()
		{
			response.StatusCode.Should().Be(200);
			List<LabelsList> mylabels = LabelsList.Deserialize(response);
			for (int i = 0; i < mylabels.Count; i++)
			{
				Console.WriteLine(mylabels[i].id, mylabels[i].idBoard, mylabels[i].name, mylabels[i].color);
			}
		}

		[When(@"the user wants to get a label with name ""(.*)"" and invalid data to boardId (.*), fields (.*) and limit (.*)")]
		public void WhenTheUserWantsToGetALabelWithNameAndInvalidDataToBoardIdFieldsAndLimit(string name, string boardId, string fields, string limit)
		{
		request = new RestRequest(getBoards + boardId + "/labels");
			request.AddHeader("Accept", "application/json");
			request.AddQueryParameter("key", key);
			request.AddQueryParameter("token", token);
			request.AddQueryParameter("name", name);
			request.AddQueryParameter("fields", fields);
			request.AddQueryParameter("limit", limit);
		response = client.Post(request);

		}

>>>>>>> Silvana
	}

}

