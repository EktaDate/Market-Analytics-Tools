using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketSummaryConsole
{
    public class ProspectSummaryData
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "prospectname")]
        public string ProspectName { get; set; }

        [JsonProperty(PropertyName = "searchstring")]
        public string SearchString { get; set; }


        [JsonProperty(PropertyName = "searchresult")]
        public string SearchResult { get; set; }

        [JsonProperty(PropertyName = "twitterupdates")]        
        public bool TwitterUpdates { get; set; }

        [JsonProperty(PropertyName = "fbupdates")]        
        public bool FacebookUpdates { get; set; }

        [JsonProperty(PropertyName = "linkedinupdates")]        
        public bool LinkedinUpdates { get; set; }

        [JsonProperty(PropertyName = "emailupdates")]        
        public bool EmailUpdates { get; set; }

        [JsonProperty(PropertyName = "bingsearchupdates")]        
        public bool BingSearchUpdates { get; set; }

        [JsonProperty(PropertyName = "isprocessed")]
        public bool IsProcessed { get; set; }

        [JsonProperty(PropertyName = "dataprocesseddate")]
        public DateTime? DataProcessedDate { get; set; }


    }
}
