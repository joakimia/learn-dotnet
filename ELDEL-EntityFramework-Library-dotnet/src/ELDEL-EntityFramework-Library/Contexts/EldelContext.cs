using ELDEL_EntityFramework_Library.Models;
using ELDEL_EntityFramework_Library.Models.Configurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ELDEL_EntityFramework_Library
{
    public class EldelContext : IdentityDbContext<Account>
    {
        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<Car> Cars { get; set; }
        public virtual DbSet<Charger> Chargers { get; set; }
        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<AccountRole> AccountRoles { get; set; }

        public EldelContext(DbContextOptions<EldelContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure ASP.net Identity before custom / eldel-defined entities.
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new AddressConfiguration());
            modelBuilder.ApplyConfiguration(new CarConfiguration());
            modelBuilder.ApplyConfiguration(new ChargerConfiguration());
            modelBuilder.ApplyConfiguration(new ChargerDetailsConfiguration());
            modelBuilder.ApplyConfiguration(new AccountConfiguration());
            modelBuilder.ApplyConfiguration(new AccountRoleConfiguration());
        }
    }
}
