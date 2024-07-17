using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<ContactCategory> ContactCategories { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresEnum<CategoryType>();

            modelBuilder.Entity<Contact>()
                .HasIndex(c => c.Email)
                .IsUnique();

            modelBuilder.Entity<ContactCategory>().HasData(
               new ContactCategory { Id = 1, Name = "prywatny", Type = CategoryType.PERSONAL },
               new ContactCategory { Id = 2, Name = "szef", Type = CategoryType.WORK },
               new ContactCategory { Id = 3, Name = "klient", Type = CategoryType.WORK }
            );
        }
    }
}