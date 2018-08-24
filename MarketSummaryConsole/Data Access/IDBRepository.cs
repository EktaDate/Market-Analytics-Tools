using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MarketSummaryConsole
{
    public interface IDBRepository
    {
        Task<IEnumerable<ProspectSearchCriteria>> GetProspectSearchCriteriaAsync();

        Task<bool> CreateProspectDataAsync(ProspectSummaryData prospectData);
        
    }
}
