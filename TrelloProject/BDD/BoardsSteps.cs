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

		[When(@"the user wants to get the board ""(.*)""")]
		public void WhenTheUserWantsToGetTheBoard(string boardId)
		{
			request = new RestRequest(getBoards + boardId);
			request.AddHeader("Accept", "application/json");
			request.AddQueryParameter("key", key);
			request.AddQueryParameter("token", token);
			response = client.Get(request);
		}

		[When(@"the user wants to get the owning boards")]
		public void WhenTheUserWantsToGetTheOwningBoards()
		{
			request = new RestRequest(getListOfBoards + userId + "/boards");
			request.AddHeader("Accept", "application/json");
			request.AddQueryParameter("key", key);
			request.AddQueryParameter("token", token);
			response = client.Get(request);
		}

		[When(@"gets one of the boards")]
		public void WhenGetsOneOfTheBoards()
		{
			List<BoardList> myBoards = BoardList.Deserialize(response);
			_boardId = myBoards[rdm.Next(0, myBoards.Count - 1)].id;

			request = new RestRequest(getBoards + _boardId);
			request.AddHeader("Accept", "application/json");
			request.AddQueryParameter("key", key);
			request.AddQueryParameter("token", token);
			response = client.Get(request);
		}


		[Then(@"the board with ""(.*)"" is retrieved")]
		public void ThenTheBoardWithIsRetrieved(string boardId)
		{
			response.StatusCode.Should().Be(200);
			Board myBoard = Board.Deserialize(response);
			myBoard.id.Should().Be(boardId);
		}

		[Then(@"an unauthorized error with text ""(.*)"" shows")]
		public void ThenAnUnauthorizedErrorWithTextShows(string errorMessage)
		{
			response.StatusCode.Should().Be(401);
			response.Content.Should().Be(errorMessage);
		}

		[Then(@"a request error with text ""(.*)"" shows")]
		public void ThenARequestErrorWithTextShows(string errorMessage)
		{
			response.StatusCode.Should().Be(400);
			response.Content.Should().Be(errorMessage);
		}

		[Then(@"the board selected board is retrieved")]
		public void ThenTheBoardSelectedBoardIsRetrieved()
		{
			response.StatusCode.Should().Be(200);
			Board myBoard = Board.Deserialize(response);
			myBoard.id.Should().Be(_boardId);
		}

		[Then(@"a found error with text ""(.*)"" shows")]
		public void ThenAFoundErrorWithTextShows(string errorMessage)
		{
			response.StatusCode.Should().Be(404);
			response.Content.Should().Be(errorMessage);
		}

	}
}
