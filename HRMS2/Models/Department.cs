using Microsoft.Extensions.Options;
using System.Runtime.InteropServices;

namespace HRMS2.Models
{
    public class Department
    {
       public long Id { get; set; }
       public string Name { get; set; }
       public string Description { get; set; }
       public int ? FloorNumber { get; set; }
    }
}
