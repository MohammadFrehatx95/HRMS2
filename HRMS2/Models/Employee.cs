using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRMS2.Models
{
    public class Employee
    {
        [Key]
        public long Id { get; set; }
        [MaxLength(50)]
        public string FirstName { get; set; }

        [MaxLength(50)]
        public string LastName { get; set; }

        [MaxLength(50)]
        public string? Email { get; set; }
        [MaxLength(50)]
        public DateTime? BirthDate { get; set; }

        [ForeignKey("Department")]
        public long? DepartmentId { get; set; }
        public Department? Department { get; set; } // Navigation Property
        public decimal Salary { get; set; }

        [ForeignKey("Manager")]
        public long? ManagerId { get; set; }
        public Employee? Manager { get; set; } // Navigation Property for self-referencing

        [ForeignKey("Lookup")]
        public long PositionId { get; set; }
        public Lookup? lookup { get; set; }
    }
}
