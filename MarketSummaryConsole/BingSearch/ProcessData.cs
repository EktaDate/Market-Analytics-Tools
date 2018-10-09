using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.IO;
using System.Configuration;

namespace MarketSummaryConsole
{
    public class ProcessData
    {        
        public async Task ProcessBingSearchData()
        {                       
            IDataAccess dataAccess = DataAccess.GetInstance();            
            IEnumerable<ProspectSearchCriteria> searchCriteriaList = await dataAccess.GetProspectSearchCriteriaAsync();

            foreach (ProspectSearchCriteria searchCriteria in searchCriteriaList)
            {
                string searchString = searchCriteria.ProspectName + " + " + searchCriteria.SearchString;
                BingSearchResult searchResult = BingSearch.WebSearch(searchString);                
                InsertBingDataAsync(searchResult.jsonResult, searchCriteria.ProspectName, searchCriteria.SearchString).Wait();                
            }                      
        }

        public async Task ProcessEmailData(string fileName)
        {
            ProspectSummaryData prospectSummaryData = new ProspectSummaryData
            {
                ProspectName = "",
                SearchResult = readBlobData(fileName),
                SearchString = "",
                EmailUpdates= true,
                DataProcessedDate = null
            };
            IDataAccess dataAccess = DataAccess.GetInstance();
            await dataAccess.InsertProspectData(prospectSummaryData);
        }

        private string readBlobData(string filename)
        {
            try
            {                
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["tableStorageConnection"]);                
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();                
                CloudBlobContainer container = blobClient.GetContainerReference("emailbody");                
                CloudBlockBlob blockBlob2 = container.GetBlockBlobReference(filename);

                string text = string.Empty;
                using (var memoryStream = new MemoryStream())
                {
                    blockBlob2.DownloadToStream(memoryStream);
                    text = System.Text.Encoding.UTF8.GetString(memoryStream.ToArray());
                }

                return text;
            }
            catch
            {
                return string.Empty;
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
