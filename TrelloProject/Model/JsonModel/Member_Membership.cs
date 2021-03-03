using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace TrelloProject.Model.JsonModel
{
	public class Member_Membership
	{
		public string id { get; set; }
		public List<Member> members { get; set; }
		public List<Membership> memberships { get; set; }

		public static Member_Membership Deserialize(IRestResponse response)
		{
			Member_Membership repoReponse = JsonConvert.DeserializeObject<Member_Membership>(response.Content.ToString());
			return repoReponse;
		}
	}
}
