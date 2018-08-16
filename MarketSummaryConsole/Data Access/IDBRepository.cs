using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MarketSummaryConsole
{
    public interface IDBRepository<T> where T: class
    {
        Task<IEnumerable<T>> GetProspectsAsync(Expression<Func<T, bool>> predicate);
        
        Task<bool> CreateDataAsync(T data);       
    }
}
