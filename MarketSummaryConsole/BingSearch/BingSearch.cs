using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MarketSummaryConsole
{
    public static class BingSearch
    {
        static readonly string congnitiveaccessKey = ConfigurationManager.AppSettings["congnitiveaccessKey"];
        static readonly string uriBase = ConfigurationManager.AppSettings["bingsearchuri"];
        static readonly string recordCount = ConfigurationManager.AppSettings["bingurlcount"];
        static readonly string ageOfdataInMonths = ConfigurationManager.AppSettings["bingsearchageofdata"];

        /// <summary>
        /// Performs a Bing Web search and return the results as a SearchResult.
        /// </summary>
        public static BingSearchResult WebSearch(string searchQuery)
        {   
            if (congnitiveaccessKey.Length == 32)
            {                
                string ageOfData = DateTime.Now.AddMonths(-int.Parse(ageOfdataInMonths)).ToString("yyyy-MM-dd") +".."+ DateTime.Now.ToString("yyyy-MM-dd"); 
                var uriQuery = uriBase + "?q=" + Uri.EscapeDataString(searchQuery) + "&count=" + recordCount + "&freshness=" + ageOfData;
                WebRequest request = HttpWebRequest.Create(uriQuery);

                request.Headers["Ocp-Apim-Subscription-Key"] = congnitiveaccessKey;
                HttpWebResponse response = (HttpWebResponse)request.GetResponseAsync().Result;
                string json = new StreamReader(response.GetResponseStream()).ReadToEnd();

                // Create result object for return
                var searchResult = new BingSearchResult()
                {
                    jsonResult = json,
                    relevantHeaders = new Dictionary<String, String>()
                };
                
                // Extract Bing HTTP headers
                foreach (String header in response.Headers)
                {
                    if (header.StartsWith("BingAPIs-") || header.StartsWith("X-MSEdge-"))
                        searchResult.relevantHeaders[header] = response.Headers[header];
                }                
                return searchResult;
            }
            else
            {
                Console.WriteLine("Invalid Bing Search API subscription key!");
                Console.WriteLine("Please paste yours into the source code.");
            }

            return null;
        }
    }

}
