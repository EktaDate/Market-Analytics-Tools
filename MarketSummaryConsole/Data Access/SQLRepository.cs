using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using System.Configuration;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Net;
using System.Data;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System;
using MarketSummaryConsole.Model;
using AutoMapper;
using System.Data.Entity;
using System.Linq;

namespace MarketSummaryConsole
{
    public class SQLRepository<T> : IDBRepository<T> where T : class
    {
        ProspectDBContext prospectDBContext = new ProspectDBContext();

        public async Task<IEnumerable<T>> GetProspectsAsync(Expression<Func<T, bool>> predicate)
        {
            IEnumerable<ProspectDataSearchCriteria> prospectDataSearchCriteria = await prospectDBContext.ProspectDataSearchCriterias.ToListAsync<ProspectDataSearchCriteria>();
            IEnumerable<T> prospectSearchCriteriaList = Mapper.Map<IEnumerable<ProspectDataSearchCriteria>, IEnumerable<T>>(prospectDataSearchCriteria);
            return prospectSearchCriteriaList.AsQueryable().Where(predicate);

        }
        
        public async Task<bool> CreateDataAsync(T data)
        {
            try
            {
                ProspectData prospectData = Mapper.Map<T, ProspectData>(data);
                prospectDBContext.ProspectDatas.Add(prospectData);
                int result = await prospectDBContext.SaveChangesAsync();
                return result < 1 ? false : true;
            }
            catch(Exception ex)
            {
                return false;
            }
            
        }               
    }
}
