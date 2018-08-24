using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketSummaryConsole
{
    public class ProspectSearchCriteria : TableEntity
    {
        [JsonProperty(PropertyName = "id")]
        [IgnoreProperty]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "prospectname")]

        public string ProspectName { get; set; }

        [JsonProperty(PropertyName = "searchstring")]
        public string SearchString { get; set; }

        [JsonProperty(PropertyName = "twitterupdates")]
        [DisplayName("Twitter Updates")]
        public bool TwitterUpdates { get; set; }

        [JsonProperty(PropertyName = "fbupdates")]
        [DisplayName("Facebook Updates")]
        public bool FacebookUpdates { get; set; }

        [JsonProperty(PropertyName = "linkedinupdates")]
        [DisplayName("LinkedIn Updates")]
        public bool LinkedinUpdates { get; set; }

        [JsonProperty(PropertyName = "emailupdates")]
        [DisplayName("Email Updates")]
        public bool EmailUpdates { get; set; }

        [JsonProperty(PropertyName = "bingsearchupdates")]
        [DisplayName("Bing Search")]
        public bool BingSearchUpdates { get; set; }
    }
  
}
