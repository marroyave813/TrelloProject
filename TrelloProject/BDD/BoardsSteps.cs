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
		string _labelsId;
		string _labelsColor;
		string _labelsName;

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




		[When(@"the user wants to get the private board ""(.*)""")]
		public void WhenTheUserWantsToGetThePrivateBoard(string boardId)
		{
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

