using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace HRMS2.Models
{
    public class Department
    {
       public long Id { get; set; }

       [MaxLength(50)]
       public string Name { get; set; }
       public string Description { get; set; }
       public int ? FloorNumber { get; set; }
    }
}
