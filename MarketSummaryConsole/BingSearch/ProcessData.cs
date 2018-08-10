using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MarketSummaryConsole
{
    public static class ProcessData
    {

        public static async Task ProcessBingSearchData()
        {
         
            //IEnumerable<ProspectDataSearchCriteria> searchCriterias = await CosmosRepository<ProspectDataSearchCriteria>.GetAllProspectsDataAsync(p => p.BingSearchUpdates == true);

            SQLRepository sqlDataAccess = new SQLRepository();
            string WhereClause = " Where BingSearchUpdates = 'true'";

            IEnumerable<ProspectDataSearchCriteria> searchCriterias  = sqlDataAccess.GetProspectData(WhereClause);
            foreach (ProspectDataSearchCriteria searchCriteria in searchCriterias)
            {
                string searchString = searchCriteria.ProspectName + " + " + searchCriteria.SearchString;
                BingSearchResult searchResult = BingSearch.WebSearch(searchString, "20","Month");                
                InsertBingDataAsync(searchResult, searchCriteria).Wait();
            }                      
        }

        public static async Task InsertBingDataAsync(BingSearchResult searchResults, ProspectDataSearchCriteria prospectSearchCriteria)
        {
          
            var allSearchResults = JObject.Parse(searchResults.jsonResult);                                                 
            List<ExtractedBingSearchData> searchResultList = new List<ExtractedBingSearchData>();
            ExtractedBingSearchData searchData = null;

            foreach (var searchResult in allSearchResults["webPages"]["value"])
            {
                searchData = new ExtractedBingSearchData();
                searchData.url = Convert.ToString(searchResult["url"]);
                searchResultList.Add(searchData);
            }

            string jsonUrlString = JsonConvert.SerializeObject(searchResultList);

            ProspectData searchProspectData = new ProspectData
            {
                ProspectName = prospectSearchCriteria.ProspectName,
                SearchResult = jsonUrlString,
                SearchString = prospectSearchCriteria.ProspectName + " + " + prospectSearchCriteria.SearchString,
                BingSearchUpdates = true
            };

            //await CosmosRepository<ProspectData>.CreateSearchDataAsync(searchProspectData);
            SQLRepository sqlDataAccess = new SQLRepository();
            sqlDataAccess.InsertProspectData(searchProspectData);
        }
    }
}
