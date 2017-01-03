using Newtonsoft.Json;
using Solocast.Core.Contracts;
using Solocast.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Solocast.DAL.SQLite;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore;

namespace Solocast.DAL
{
    public class SQLitePodcastService : IPodcastStore<Podcast>, IDatabaseMigrator
    {
        public void Migrate()
        {
            using (var db = new PodcastsContext())
            {
                db.Database.Migrate();
            }
        }

        public async Task<IList<Podcast>> LoadAsync()
        {
            List<Podcast> entities;

            using (var db = new PodcastsContext())
            {
                entities = await db.Podcasts.Include(x => x.Episodes).ToListAsync();
            }

            return entities ?? new List<Podcast>();
        }

        public async Task SaveAsync(IEnumerable<Podcast> entities)
        {
            using (var db = new PodcastsContext())
            {
                db.Podcasts.UpdateRange(entities);

                await db.SaveChangesAsync();
            }
        }

        public async Task SaveAsync(Podcast entity)
        {
            using (var db = new PodcastsContext())
            {
                db.Podcasts.Add(entity);

                await db.SaveChangesAsync();
            }
        }
    }
}
