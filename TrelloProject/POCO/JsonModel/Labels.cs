using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace TrelloProject.Model.JsonModel
{
    class Labels
    {
        public string id { get; set; }
        public string idBoard { get; set; }
        public string name { get; set; }
        public string color { get; set; }
        public Limits limits { get; set; }

        public static Labels Deserialize(IRestResponse response)
        {
            Labels repoReponse = JsonConvert.DeserializeObject<Labels>(response.Content.ToString());
            return repoReponse;
        }

    }
}
