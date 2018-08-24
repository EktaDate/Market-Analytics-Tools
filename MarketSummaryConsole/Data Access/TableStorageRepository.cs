using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketSummaryConsole
{
    public class TableStorageRepository : IDBRepository
    {        
        public CloudTable GetTableManager(string _CloudTableName)
        {
            CloudTable table;
            try
            {
                string ConnectionString = ConfigurationManager.AppSettings["tableStorageConnection"];
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConnectionString);
                CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

                table = tableClient.GetTableReference(_CloudTableName);
                table.CreateIfNotExists();
                return table;
            }
            catch (StorageException StorageExceptionObj)
            {
                throw StorageExceptionObj;
            }
            catch (Exception ExceptionObj)
            {
                throw ExceptionObj;
            }
        }
        
        public async Task<IEnumerable<ProspectSearchCriteria>> GetProspectSearchCriteriaAsync()
        {
            try
            {                
                string query = TableQuery.GenerateFilterConditionForBool("BingSearchUpdates", QueryComparisons.Equal, true);                
                CloudTable table = GetTableManager("ProspectSearchCriteria");
                TableQuery <ProspectSearchCriteria> tableQuery = new TableQuery<ProspectSearchCriteria>();
                tableQuery = new TableQuery<ProspectSearchCriteria>().Where(query);
                IEnumerable<ProspectSearchCriteria> prospectSearchCriteriaList = table.ExecuteQuery(tableQuery);
                return prospectSearchCriteriaList;
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> CreateProspectDataAsync(ProspectSummaryData prospectData)
        {
            try
            {
                CloudTable table = GetTableManager("ProspectData");
                prospectData.PartitionKey = "ProspectSummary";
                prospectData.RowKey = Guid.NewGuid().ToString();
                TableOperation insertOperation = TableOperation.Insert(prospectData);
                await table.ExecuteAsync(insertOperation);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}
