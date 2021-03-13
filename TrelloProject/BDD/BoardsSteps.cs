using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
		int numericStatusCode = 0;
		bool result = false;
		string responseContent;
		Random rdm = new Random();

		//API variables
		IRestClient client = new RestClient();
		IRestRequest request;
		IRestResponse response;
		HttpStatusCode statusCode;

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
                case "JQ":
					key = "2fc7585583dc33af33a0dbc8996dfc1d";
					token = "662175425929e4bef3a30be382218efafe0f4a5d10e12330bc81ff5f31295ac0";
					userId = "jqyuxi100";
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


		[When(@"the user try to get the board ""(.*)"" with error message ""(.*)""")]
		public void WhenTheUserTryToGetTheBoardWithErrorMessage(string boardId, string errorMesage)
		{
			//Set the request to get a board
			request = new RestRequest(getBoards + boardId);
			request.AddHeader("Accept", "application/json");
			request.AddQueryParameter("key", key);
			request.AddQueryParameter("token", token);
			response = client.Get(request);
			Assert.AreEqual(errorMesage, response.Content);

			statusCode = response.StatusCode;
			numericStatusCode = (int)statusCode;
		}




		[When(@"the user wants to get the private board ""(.*)""")]
		public void WhenTheUserWantsToGetThePrivateBoard(string boardId)
		{
			//Set the request to get a board
			request = new RestRequest(getBoards + boardId);
			request.AddHeader("Accept", "application/json");
			request.AddQueryParameter("key", key);
			request.AddQueryParameter("token", token);
			response = client.Get(request);
			string expectedResponse = "unauthorized permission requested";
			Assert.AreEqual(expectedResponse, response.Content);

			statusCode = response.StatusCode;
			//numericStatusCode = (int)statusCode;
		}



		[When(@"the user wants to get the board (.*)caf(.*)")]
		public void WhenTheUserWantsToGetTheBoardCaf(Decimal p0, Decimal p1)
		{
			ScenarioContext.Current.Pending();
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


		[When(@"search for a board named ""(.*)""")]
		public void WhenSearchForABoardNamed(string p0)
		{
			ScenarioContext.Current.Pending();
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

			statusCode = response.StatusCode;
			numericStatusCode = (int)statusCode;

			if (response.IsSuccessful == true && numericStatusCode == 200)
			{
				result = true;
			}
			Assert.IsTrue(result);
		}



		[When(@"delete it with Id ""(.*)"" and error message ""(.*)""")]
		public void WhenDeleteItWithIdAndErrorMessage(string IdBoard, string ServerMessage)
		{
			request = new RestRequest(getBoards + IdBoard);
			request.AddHeader("Accept", "application/json");
			request.AddQueryParameter("key", key);
			request.AddQueryParameter("token", token);
			//request.AddParameter("Id", IdBoard);
			response = client.Delete(request);
			responseContent = response.Content;


			if(responseContent.Contains(ServerMessage))
            {
				result = true;
            }
			Assert.IsTrue(result);
			result = false;
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
			result = false;
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




		[Then(@"the system should not show the board with id ""(.*)"" anymore and Status code is (.*) and ""(.*)""")]
		public void ThenTheSystemShouldNotShowTheBoardWithIdAnymoreAndStatusCodeIsAnd(string idBoard, int statusCodeErrorMessage, string errorMessage)
		{
			//Set the request to get a board
			request = new RestRequest(getBoards + idBoard);
			request.AddHeader("Accept", "application/json");
			request.AddQueryParameter("key", key);
			request.AddQueryParameter("token", token);
			response = client.Get(request);
			statusCode = response.StatusCode;
			numericStatusCode = (int)statusCode;

			if (response.IsSuccessful == false && numericStatusCode == statusCodeErrorMessage && response.Content == errorMessage)
			{
				result = true;
			}
			Assert.IsTrue(result);
		}




		[Then(@"the board is already deleted and Status code is (.*)")]
		public void ThenTheBoardIsAlreadyDeletedAndStatusCodeIs(int StatusCode)
		{
			if (response.IsSuccessful == false && numericStatusCode == StatusCode)
			{
				result = true;
			}
			Assert.IsTrue(result);
		}


		[Then(@"the system should not let delete the private board and throw a Status code ""(.*)""")]
		public void ThenTheSystemShouldNotLetDeleteThePrivateBoardAndThrowAStatusCode(string StatusCode)
		{
			{
				if (response.IsSuccessful == false && statusCode.ToString() == StatusCode)
				{
					result = true;
				}
				Assert.IsTrue(result);
			}
		}
	}
}
