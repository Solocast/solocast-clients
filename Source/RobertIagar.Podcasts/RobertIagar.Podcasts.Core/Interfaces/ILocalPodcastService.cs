using RobertIagar.Podcasts.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobertIagar.Podcasts.Core.Interfaces
{
    public interface ILocalStorageService<T>
    {
        Task SaveAsync(T entity);
        Task SaveAsync(IEnumerable<T> entities);
        Task<IList<T>> LoadAsync();
    }
}
