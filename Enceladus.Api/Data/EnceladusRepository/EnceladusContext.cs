using Enceladus.Api;
using Enceladus.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Enceladus
{
    public class EnceladusContext : DbContext
    {
        public EnceladusContext()
        {
            Database.EnsureCreated();
        }
        public DbSet<AppUser> AppUsers { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //var connString = Environment.GetEnvironmentVariable("ENCELADUS_CONNECTION_STRING");
            var connString = Startup.Configuration["ConnectionStrings:ENCELADUS_CONNECTION_STRING"];
            optionsBuilder.UseSqlServer(connString, options =>
            {
                options.EnableRetryOnFailure(5);
            });
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
        }
    }
}
