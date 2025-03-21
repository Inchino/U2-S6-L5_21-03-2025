using GestionaleHotel.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GestionaleHotel.Data
{
    public class HotelDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string,
        IdentityUserClaim<string>, ApplicationUserRole, IdentityUserLogin<string>,
        IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
        public HotelDbContext(DbContextOptions<HotelDbContext> options) : base(options) { }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        public DbSet<ApplicationRole> ApplicationRoles { get; set; }

        public DbSet<ApplicationUserRole> ApplicationUserRoles { get; set; }

        //// Tabella per i prodotti
        //public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.ApplicationUserRole)
                .HasForeignKey(ur => ur.UserId);

            modelBuilder.Entity<ApplicationUserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(u => u.ApplicationUserRole)
                .HasForeignKey(ur => ur.RoleId);

            //modelBuilder.Entity<Product>().HasOne(p => p.User).WithMany(u => u.Products).OnDelete(DeleteBehavior.Cascade);

            //modelBuilder.Entity<Product>().Property(p => p.UserId).IsRequired(false);
        }
    }
}