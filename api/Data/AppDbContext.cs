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

            // modelBuilder.Entity<Contact>().HasData(
            //     new Contact
            //     {
            //         Id = 1,
            //         FirstName = "Jan",
            //         LastName = "Kowalski",
            //         Email = "stanislaw@gmail.com",
            //         Phone = "+48 123 456 789",
            //         BirthDate = new DateOnly(1990, 1, 1),
            //         Category = new ContactCategory { Id = 3, Name = "personal", Type = CategoryType.PERSONAL },
            //         Password = "password",
            //     }
            // );

        }
    }
}