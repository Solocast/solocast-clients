using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Solocast.DAL.SQLite;

namespace Solocast.DAL.Migrations
{
    [DbContext(typeof(PodcastsContext))]
    [Migration("20161024221252_InitialMigration")]
    partial class InitialMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rtm-21431");

            modelBuilder.Entity("Solocast.Core.Contracts.Episode", b =>
                {
                    b.Property<int>("EpisodeId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Author");

                    b.Property<string>("Guid");

                    b.Property<string>("ImageUrl");

                    b.Property<string>("Path");

                    b.Property<int>("PodcastId");

                    b.Property<DateTime>("Published");

                    b.Property<string>("Subtitle");

                    b.Property<string>("Summary");

                    b.Property<string>("Title");

                    b.Property<string>("WebPath");

                    b.HasKey("EpisodeId");

                    b.HasIndex("PodcastId");

                    b.ToTable("Episodes");
                });

            modelBuilder.Entity("Solocast.Core.Contracts.Podcast", b =>
                {
                    b.Property<int>("PodcastId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Author");

                    b.Property<DateTime>("DateAdded");

                    b.Property<string>("Description");

                    b.Property<string>("FeedUrl");

                    b.Property<string>("ImageUrl");

                    b.Property<string>("Title");

                    b.HasKey("PodcastId");

                    b.ToTable("Podcasts");
                });

            modelBuilder.Entity("Solocast.Core.Contracts.Episode", b =>
                {
                    b.HasOne("Solocast.Core.Contracts.Podcast", "Podcast")
                        .WithMany("Episodes")
                        .HasForeignKey("PodcastId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
