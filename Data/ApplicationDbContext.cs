using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TvShowTrackerApi.Models;

namespace TvShowTrackerApi.Data
{
    public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions options, IOptions<OperationalStoreOptions> operationalStoreOptions)
            : base(options, operationalStoreOptions)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //Define composite constraints
            builder.Entity<Series>().HasKey(pk => new { pk.SeriesID, pk.UserID });
            builder.Entity<Episode>().HasKey(pk => new { pk.EpisodeID, pk.SeriesID, pk.UserID });
        }
        public DbSet<Series> Series { get; set; }
        public DbSet<Episode> Episodes { get; set; }
    }
}
