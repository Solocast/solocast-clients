using Solocast.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solocast.Core.Interfaces
{
    public interface IPodcastStore<T>
    {
        Task SaveAsync(T entity);
        Task SaveAsync(IEnumerable<T> entities);
        Task<IList<T>> LoadAsync();
    }
}
