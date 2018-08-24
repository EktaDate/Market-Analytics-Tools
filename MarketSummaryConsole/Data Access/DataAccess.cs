using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Linq.Expressions;
namespace MarketSummaryConsole
{
    public sealed class DataAccess : IDataAccess
    {
        private static DataAccess instance;
        private DataAccess()
        {

        }        

        public static DataAccess GetInstance()
        {
            if (instance == null)
            {
                instance = new DataAccess();
            }

            return instance;
        }

        public async Task<IEnumerable<ProspectSearchCriteria>> GetProspectSearchCriteriaAsync()
        {
            IDBRepository dbObejct = FactoryClass.CreateDBRepositoryObject();
            IEnumerable<ProspectSearchCriteria> prospectSearchCriteriaList = await dbObejct.GetProspectSearchCriteriaAsync();
            return prospectSearchCriteriaList;
        }
       
        public async Task<bool> InsertProspectData(ProspectSummaryData prospectSummary)
        {
            try
            {
                IDBRepository dbObejct = FactoryClass.CreateDBRepositoryObject();
                return await dbObejct.CreateProspectDataAsync(prospectSummary);
            }
            catch
            {
                return false;
            }
        }     
    }
}