using System;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using System.Configuration;
using Microsoft.Azure.Documents.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace MarketSummaryConsole
{
    public class CosmosRepository<T> : IDBRepository<T> where T : class
    {
        private static readonly string DatabaseId = ConfigurationManager.AppSettings["database"];
        private static readonly string CollectionId = ConfigurationManager.AppSettings["collection"];
        private static readonly string SearchCollectionId = ConfigurationManager.AppSettings["Searchcollection"];
        private static DocumentClient client;

        public static void Initialize()
        {
            client = new DocumentClient(new Uri(ConfigurationManager.AppSettings["endpoint"]), ConfigurationManager.AppSettings["authKey"]);
            CreateDatabaseIfNotExistsAsync().Wait();
            CreateCollectionIfNotExistsAsync().Wait();
        }
        
        private static async Task CreateDatabaseIfNotExistsAsync()
        {
            try
            {
                await client.ReadDatabaseAsync(UriFactory.CreateDatabaseUri(DatabaseId));
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    await client.CreateDatabaseAsync(new Database { Id = DatabaseId });
                }
                else
                {
                    throw;
                }
            }
        }

        private static async Task CreateCollectionIfNotExistsAsync()
        {
            try
            {
                await client.ReadDocumentCollectionAsync(UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId));
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    await client.CreateDocumentCollectionAsync(UriFactory.CreateDatabaseUri(DatabaseId), new DocumentCollection { Id = CollectionId },
                        new RequestOptions { OfferThroughput = 1000 });
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task<bool> CreateDataAsync(T data)
        {

            Document doc = await client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId), data);                        
            if(doc == null)
            {
                return false;
            }
            return true;
        }

        public async Task<IEnumerable<T>> GetProspectsAsync(Expression<Func<T, bool>> predicate)
        {
            try
            {
                Initialize();
                IDocumentQuery<T> query = client.CreateDocumentQuery<T>(UriFactory.CreateDocumentCollectionUri(DatabaseId, SearchCollectionId),
                new FeedOptions { MaxItemCount = -1, EnableCrossPartitionQuery = true })
                .Where(predicate)                
                .AsDocumentQuery();

                List<T> results = new List<T>();
                while (query.HasMoreResults)
                {
                    results.AddRange(await query.ExecuteNextAsync<T>());
                }

                return results;
            }
            catch (Exception e)
            {
                throw e;

            }
        }
    }
}
