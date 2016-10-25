using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Solocast.Core.Contracts;

namespace Solocast.DAL.SQLite
{
    public class PodcastsContext : DbContext
    {
        public DbSet<Podcast> Podcasts { get; set; }
        public DbSet<Episode> Episodes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
#if UNIT_TEST
            optionsBuilder.UseSqlite("Filename=PodcastsTests.db");
#else
            optionsBuilder.UseSqlite("Filename=Podcasts.db");
#endif
        }
    }
}
