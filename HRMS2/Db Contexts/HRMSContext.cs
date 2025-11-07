using HRMS2.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace HRMS2.Db_Contexts
{
    public class HRMSContext : DbContext
    {
        // base of DbContext class
        public HRMSContext(DbContextOptions<HRMSContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Lookup>().HasData(
                //Employee Position (Major Code = 0)ش
                new Lookup { Id = 1, MajorCode = 0, MinorCode = 0, Name = "Employee Position" },
                new Lookup { Id = 2, MajorCode = 0, MinorCode = 1, Name = "Developer" },
                new Lookup { Id = 3, MajorCode = 0, MinorCode = 2, Name = "HR" },
                new Lookup { Id = 4, MajorCode = 0, MinorCode = 3, Name = "Manager" },

                //Department Types (Major Code = 1)
                new Lookup { Id = 5, MajorCode = 1, MinorCode = 0, Name = "Department Type" },
                new Lookup { Id = 6, MajorCode = 1, MinorCode = 1, Name = "Finance" },
                new Lookup { Id = 7, MajorCode = 1, MinorCode = 2, Name = "Adminstrative" },
                new Lookup { Id = 8, MajorCode = 1, MinorCode = 3, Name = "Technical" }

            );
        }
        //tables
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Lookup> Lookups { get; set; }
    }
}
