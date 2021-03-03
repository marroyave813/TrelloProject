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
	}
}
