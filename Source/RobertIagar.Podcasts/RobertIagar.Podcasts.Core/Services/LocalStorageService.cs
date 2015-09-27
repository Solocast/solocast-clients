using Newtonsoft.Json;
using RobertIagar.Podcasts.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace RobertIagar.Podcasts.Core.Services
{
    public class LocalStorageService<T> : ILocalStorageService<T>
    {
        private string filename;
        private IStorageFolder localFolder;

        public LocalStorageService(string filename)
        {
            this.localFolder = ApplicationData.Current.LocalFolder;
            this.filename = filename;
        }

        public async Task<IList<T>> LoadAsync()
        {
            var file = await localFolder.CreateFileAsync(filename, CreationCollisionOption.OpenIfExists);
            var items = await FileIO.ReadTextAsync(file);

            var entities = JsonConvert.DeserializeObject<IList<T>>(items);
            return entities;
        }

        public async Task SaveAsync(IEnumerable<T> entities)
        {
            var items = JsonConvert.SerializeObject(entities);
            var file = await localFolder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);

            await FileIO.WriteTextAsync(file, items);
        }

        public async Task SaveAsync(T entity)
        {
            var items = await LoadAsync();
            items.Add(entity);

            await SaveAsync(items);
        }
    }
}
