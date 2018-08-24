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
    public class SQLRepository : IDBRepository
    {
        ProspectDBContext prospectDBContext = new ProspectDBContext();

        public async Task<IEnumerable<ProspectSearchCriteria>> GetProspectSearchCriteriaAsync()
        {

            IEnumerable<ProspectDataSearchCriteria> prospectDataSearchCriteria = await prospectDBContext.ProspectDataSearchCriterias.Where(p=>p.BingSearchUpdates == true).ToListAsync<ProspectDataSearchCriteria>();
            IEnumerable<ProspectSearchCriteria> prospectSearchCriteriaList = Mapper.Map<IEnumerable<ProspectDataSearchCriteria>, IEnumerable<ProspectSearchCriteria>>(prospectDataSearchCriteria);
            return prospectSearchCriteriaList;
        }        

        public async Task<bool> CreateProspectDataAsync(ProspectSummaryData prospectSummaryData)
        {
            try
            {
                ProspectData prospectData = Mapper.Map<ProspectSummaryData, ProspectData>(prospectSummaryData);
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
