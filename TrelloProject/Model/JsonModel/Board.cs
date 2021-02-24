using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace TrelloProject.Model.JsonModel
{
    public class Board
    {
        public string id { get; set; }
        public string name { get; set; }
        public string desc { get; set; }
        public object descData { get; set; }
        public bool closed { get; set; }
        public string idOrganization { get; set; }
        public object idEnterprise { get; set; }
        public bool pinned { get; set; }
        public string url { get; set; }
        public string shortUrl { get; set; }
        public LabelNames labelNames { get; set; }
        public Prefs prefs { get; set; }

        public static Board Deserialize(IRestResponse response)
        {
            Board repoReponse = JsonConvert.DeserializeObject<Board>(response.Content.ToString());
            return repoReponse;
        }
    }

    
}
