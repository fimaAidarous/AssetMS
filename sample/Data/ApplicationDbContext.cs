using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using sample.Models;

namespace sample.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Asset> Assets { get; set; }
        public DbSet<Asset_Movement> AssetMovements { get; set; }
        public DbSet<Asset_Maintenance> AssetMaintenances { get; set; }
        // public DbSet<User> Users { get; set; }
    }
}
