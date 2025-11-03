using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRMS2.Models
{
    public class Employee
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Email { get; set; }
        public string Position { get; set; }
        public DateTime? BirthDate { get; set; }

        [ForeignKey("Department")]
        public long? DepartmentId { get; set; }
        public Department? Department { get; set; } // Navigation Property
        public decimal Salary { get; set; }

        [ForeignKey("Manager")]
        public long? ManagerId { get; set; }
        public Employee? Manager { get; set; } // Navigation Property for self-referencing
    }
}
