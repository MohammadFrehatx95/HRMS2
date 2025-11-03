using HRMS2.Models;
using Microsoft.EntityFrameworkCore;

namespace HRMS2.Db_Contexts
{
    public class HRMSContext : DbContext
    {
        // base of DbContext class
        public HRMSContext(DbContextOptions<HRMSContext> options) : base(options)
        {

        }

        //tables
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
    }
}
