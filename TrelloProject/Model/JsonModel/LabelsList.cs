using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace TrelloProject.Model.JsonModel
{
    class LabelsList
    {
        public string id { get; set; }
        public string idBoard { get; set; }
        public string name { get; set; }
        public string color { get; set; }
        public Limits limits { get; set; }

        public static List <LabelsList> Deserialize(IRestResponse response)
        {
            List <LabelsList> repoReponse = JsonConvert.DeserializeObject<List<LabelsList>>(response.Content.ToString());
            return repoReponse;
        }

    }
}
