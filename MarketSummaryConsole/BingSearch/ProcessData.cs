using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Linq;
using System.Linq.Expressions;

namespace MarketSummaryConsole
{
    public class ProcessData
    {        
        public async Task ProcessBingSearchData()
        {                       
            IDataAccess dataAccess = DataAccess.GetInstance();
            Expression<Func<ProspectSearchCriteria, bool>> predicate = (p => p.BingSearchUpdates == true);                                                                               
            IEnumerable<ProspectSearchCriteria> searchCriteriaList = await dataAccess.GetProspectSearchCriteriaAsync(predicate);

            foreach (ProspectSearchCriteria searchCriteria in searchCriteriaList)
            {
                string searchString = searchCriteria.ProspectName + " + " + searchCriteria.SearchString;
                BingSearchResult searchResult = BingSearch.WebSearch(searchString);                
                InsertBingDataAsync(searchResult.jsonResult, searchCriteria.ProspectName, searchCriteria.SearchString).Wait();                
            }                      
        }
        
        private async Task InsertBingDataAsync(string bingSearchJsonResult,string prospectName, string searchString)
        {
          
            var allSearchResults = JObject.Parse(bingSearchJsonResult);                                                 
            List<ExtractedBingSearchData> searchResultList = new List<ExtractedBingSearchData>();
            ExtractedBingSearchData searchData = null;

            foreach (var searchResult in allSearchResults["webPages"]["value"])
            {
                searchData = new ExtractedBingSearchData();
                searchData.url = Convert.ToString(searchResult["url"]);                
                searchData.dateLastCrawled = Convert.ToDateTime(searchResult["dateLastCrawled"]).ToString("dd-MM-yyyy");
                searchResultList.Add(searchData);                
            }

            string jsonUrlString = JsonConvert.SerializeObject(searchResultList);

            ProspectSummaryData prospectSummaryData = new ProspectSummaryData
            {
                ProspectName = prospectName,
                SearchResult = jsonUrlString,
                SearchString = prospectName + " + " + searchString,
                BingSearchUpdates = true,
                DataProcessedDate = null
            };
            IDataAccess dataAccess = DataAccess.GetInstance();
            await dataAccess.InsertProspectData(prospectSummaryData);
        }
    }
}
