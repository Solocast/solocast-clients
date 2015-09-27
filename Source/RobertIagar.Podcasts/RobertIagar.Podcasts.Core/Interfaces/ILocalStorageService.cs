using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobertIagar.Podcasts.Core.Interfaces
{
    public interface ILocalStorageService<TEntity>
    {
        Task SaveAsync(TEntity entity);
        Task SaveAsync(IEnumerable<TEntity> entities);
        Task<IList<TEntity>> LoadAsync();
    }
}
