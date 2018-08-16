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

        public async Task<IEnumerable<ProspectSearchCriteria>> GetProspectSearchCriteriaAsync(Expression<Func<ProspectSearchCriteria, bool>> predicate)
        {
            IDBRepository<ProspectSearchCriteria> dbObejct = FactoryClass<ProspectSearchCriteria>.CreateDBRepositoryObject();
            IEnumerable<ProspectSearchCriteria> prospectSearchCriteriaList = await dbObejct.GetProspectsAsync(predicate);
            return prospectSearchCriteriaList;
        }
       
        public async Task<bool> InsertProspectData(ProspectSummaryData prospectSummary)
        {
            try
            {
                IDBRepository<ProspectSummaryData> dbObejct = FactoryClass<ProspectSummaryData>.CreateDBRepositoryObject();
                return await dbObejct.CreateDataAsync(prospectSummary);
            }
            catch
            {
                return false;
            }
        }     
    }
}