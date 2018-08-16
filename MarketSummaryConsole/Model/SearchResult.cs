using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketSummaryConsole
{
   public  class BingSearchResult
    {
        public String jsonResult;
        public Dictionary<String, String> relevantHeaders;
    }

    public class ExtractedBingSearchData
    {
        public string url;
        public string dateLastCrawled;
    }
}
