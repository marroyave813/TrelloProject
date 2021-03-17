using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace TrelloProject.Model.JsonModel
{
    public class Member
    {
        public string id { get; set; }
        public string username { get; set; }
        public bool confirmed { get; set; }
        public string memberType { get; set; }
        public bool activityBlocked { get; set; }
        public string avatarHash { get; set; }
        public string avatarUrl { get; set; }
        public string fullName { get; set; }
        public object idMemberReferrer { get; set; }
        public string initials { get; set; }
        public NonPublic nonPublic { get; set; }
        public bool nonPublicAvailable { get; set; }

        public static List<Member> Deserialize(IRestResponse response)
        {
            List<Member> repoReponse = JsonConvert.DeserializeObject<List<Member>>(response.Content.ToString());
            return repoReponse;
        }
    }

    
}
