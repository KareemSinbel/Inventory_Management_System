using InventorySystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace InventorySystem.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<StockReport> StockReports { get; set; }
        public DbSet<AlertReport> AlertReports { get; set; }


        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<ApplicationUser>().ToTable("Users", "Security").Ignore(x=> x.TwoFactorEnabled);
            modelBuilder.Entity<IdentityRole>().ToTable("Roles", "Security");
            modelBuilder.Entity<IdentityUserRole<string>>().ToTable("UserRoles", "Security");
            modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims", "Security");
            modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins", "Security");
            modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims", "Security");
            modelBuilder.Entity<IdentityUserToken<string>>().ToTable("UserTokens", "Security");


            modelBuilder.Entity<Employee>()
                .HasOne(e=> e.User)
                .WithOne(e=> e.Employee)
                .HasForeignKey<Employee>(e=>e.UserId);

            modelBuilder.Entity<Employee>()
                .HasIndex(e=>e.UserId)
                .IsUnique();
        }
    }
}
