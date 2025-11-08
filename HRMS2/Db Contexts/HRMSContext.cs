using HRMS.Models;
using HRMS2.Models;
using Microsoft.EntityFrameworkCore;

namespace HRMS.DbContexts
{
    public class HRMSContext : DbContext
    {
        public HRMSContext(DbContextOptions<HRMSContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder); // Call Parent Method

            // Seeding Database
            modelBuilder.Entity<Lookup>().HasData(
                    // Employee Postions (Major Code = 0)
                    new Lookup { Id = 1, MajorCode = 0, MinorCode = 0, Name = "Employee Positions" },
                    new Lookup { Id = 2, MajorCode = 0, MinorCode = 1, Name = "Developer" },
                    new Lookup { Id = 3, MajorCode = 0, MinorCode = 2, Name = "HR" },
                    new Lookup { Id = 4, MajorCode = 0, MinorCode = 3, Name = "Manager" },

                    // Department Types (Major Code = 1)
                    new Lookup { Id = 5, MajorCode = 1, MinorCode = 0, Name = "Department Types" },
                    new Lookup { Id = 6, MajorCode = 1, MinorCode = 1, Name = "Finance" },
                    new Lookup { Id = 7, MajorCode = 1, MinorCode = 2, Name = "Adminstrative" },
                    new Lookup { Id = 8, MajorCode = 1, MinorCode = 3, Name = "Technical" }

                );

            // Username is Unique
            modelBuilder.Entity<User>().HasIndex(x => x.Username).IsUnique();

            // Seeding Database
            // BCrypt.Net.BCrypt.HashPassword("Admin@123") = $2a$11$FodwrXysOiJ9lFlf1PZGZOQZH1fvBzBivVnSewumv5QTqlDIXh1/e
            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Username = "Admin", IsAdmin = true, HashedPassword = "$2a$11$FodwrXysOiJ9lFlf1PZGZOQZH1fvBzBivVnSewumv5QTqlDIXh1/e" }
                );
        }

        // Tables
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Lookup> Lookups { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
