using HRMS.DbContexts;
using HRMS2.Dtos.Departments;
using HRMS2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRMS2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        //public static List<Department> departments = new List<Department>()
        //{
        //    new Department() { Id = 1, Name = "IT", Description = "Information Technology", FloorNumber = 5 },
        //    new Department() { Id = 2, Name = "HR", Description = "Human Resources", FloorNumber = 3 },
        //    new Department() { Id = 3, Name = "Finance", Description = "Financial Department", FloorNumber = 4 },
        //    new Department() { Id = 4, Name = "Marketing", Description = "Marketing and Sales", FloorNumber = 2 }
        //};

        //Dependency Injection --> Container in Program.cs
        HRMSContext _dbContext;
        public DepartmentsController(HRMSContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("GetAll")]

        public IActionResult GetAll([FromQuery] FilterDepartmentDto departmentDto)
        {
            var result = from department in _dbContext.Departments
                         where (departmentDto.Name == null || department.Name.ToUpper().Contains(departmentDto.Name.ToUpper()) && (departmentDto.FloorNumber == 0 || department.FloorNumber == departmentDto.FloorNumber))
                         orderby department.Id descending
                         select new DepartmentDto
                         {
                             Id = department.Id,
                             Name = department.Name,
                             Description = department.Description,
                             FloorNumber = department.FloorNumber
                         };

            return Ok(result);
        }

        [HttpGet("GetById/{id}")]
        public IActionResult GetById([FromQuery] long id)
        {
            if (id <= 0)
            {
                return BadRequest("Id Value Is Invalid!");
            }

            var result = _dbContext.Departments.Select(x => new DepartmentDto
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                FloorNumber = x.FloorNumber
            }).FirstOrDefault(x => x.Id == id);


            if (result == null)
            {
                return NotFound($"Department with Id {id} not found.");
            }

            return Ok(result);
        }

        [HttpPost("Add")]

        public IActionResult Add([FromBody] SaveDepartmentDto DepartmentDto)
        {
            var newDepartment = new Department
            {
                //Id = (departments.LastOrDefault()?.Id ?? 0) + 1,
                Name = DepartmentDto.Name,
                Description = DepartmentDto.Description,
                FloorNumber = DepartmentDto.FloorNumber
            };

            _dbContext.Departments.Add(newDepartment);
            _dbContext.SaveChanges();
            return Ok();

        }

        [HttpPut("Update")]
        public IActionResult Update([FromBody] SaveDepartmentDto DepartmentDto)
        {
            var existingDepartment = _dbContext.Departments.FirstOrDefault(d => d.Id == DepartmentDto.Id);

            if (existingDepartment == null)
            {
                return NotFound("$The Department With Id {DepartmentDto.Id} Not Found!");
            }

            existingDepartment.Name = DepartmentDto.Name;
            existingDepartment.Description = DepartmentDto.Description;
            existingDepartment.FloorNumber = DepartmentDto.FloorNumber;

            _dbContext.SaveChanges();
            return Ok();
        }

        [HttpDelete("Delete/{id}")]
        public IActionResult Delete([FromQuery] long id)
        {
            var existingDepartment = _dbContext.Departments.FirstOrDefault(d => d.Id == id);

            if (existingDepartment == null)
            {
                return NotFound("$The Department With Id {DepartmentDto.Id} Not Found!");
            }

            _dbContext.Departments.Remove(existingDepartment);
            _dbContext.SaveChanges();

            return Ok();

        }




    }
}
