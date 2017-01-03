using Newtonsoft.Json;
using Solocast.Core.Contracts;
using Solocast.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Solocast.DAL
{
    public class LocalPodcastService : IPodcastStore<Podcast>
    {
        private string filename;
        private IStorageFolder localFolder;

        public LocalPodcastService(string filename)
        {
            this.localFolder = ApplicationData.Current.LocalFolder;
            this.filename = filename;
        }

        public async Task<IList<Podcast>> LoadAsync()
        {
            var file = await localFolder.CreateFileAsync(filename, CreationCollisionOption.OpenIfExists);
            var items = await FileIO.ReadTextAsync(file);

            var entities = JsonConvert.DeserializeObject<IList<Podcast>>(items);
            return entities ?? new List<Podcast>();
        }

        public async Task SaveAsync(IEnumerable<Podcast> entities)
        {
            var items = JsonConvert.SerializeObject(entities);
            var file = await localFolder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);

            await FileIO.WriteTextAsync(file, items);
        }

        public async Task SaveAsync(Podcast entity)
        {
            var items = await LoadAsync();
            items.Add(entity);

            await SaveAsync(items);
        }
    }
}
