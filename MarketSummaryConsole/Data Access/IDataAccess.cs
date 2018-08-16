using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MarketSummaryConsole
{
    interface IDataAccess
    {                
        Task<IEnumerable<ProspectSearchCriteria>> GetProspectSearchCriteriaAsync(Expression<Func<ProspectSearchCriteria, bool>> predicate);

        Task<bool> InsertProspectData(ProspectSummaryData prospectSummary);
    }
}
