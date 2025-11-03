using HRMS2.Db_Contexts;
using HRMS2.Dtos.Employees;
using HRMS2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.ExceptionServices;

namespace HRMS2.Controllers
{
    [Route("api/[controller]")] // data annotation
    [ApiController] // data annotation
    public class EmployeesController : ControllerBase // inherit perm to be controller
    {
        //public static List<Employee> employees = new List<Employee>()
        //{
        //  new Employee() { Id = 1,FirstName = "Murad",LastName = "Frehat",Email = "mohammadfrehat95@gmail.com",Position ="Leader", BirthDate = new DateTime(2003,11,27)},
        //  new Employee() { Id = 3,FirstName = "3amr",LastName = "Alghazo",Position ="Manager", BirthDate = new DateTime(2004,12,3) },
        //  new Employee() { Id = 3,FirstName = "Salem",LastName = "Qudah",Position ="Hr", BirthDate = new DateTime(1997,5,27) },
        //  new Employee() { Id = 4,FirstName = "Obada",LastName = "Ananbeh",Email = "obadaananbeh5@gmail.com",Position ="Co-Owner", BirthDate = new DateTime(1999,6,4) }
        //};


        //Dependency Injection --> Container in Program.cs
        private readonly HRMSContext _dbContext;
        public EmployeesController(HRMSContext dbContext)
        {
            _dbContext = dbContext; 
        }

        // name in .net for libaray is (nuget package)

        //CRUD Operations
        // C : Create --> post
        // R : Read   --> get
        // U : Update --> put
        // D : Delete --> Delete

        [HttpGet("GetByCiteria")]
        // u can't use complex data type with Get only if u use [FromQuery]
        public IActionResult GetByCiteria([FromQuery] SearchEmployeeDto employeeDto) // optional or nullable
        {
            var result = from employee in _dbContext.Employees
                         from Departments in _dbContext.Departments.Where(x => x.Id == employee.DepartmentId).DefaultIfEmpty() // left join bc defaultifempty
                         from manager in _dbContext.Employees.Where(x => x.Id == employee.ManagerId).DefaultIfEmpty() // self join
                         where (employeeDto.Position == null || employee.Position.ToUpper().Contains(employeeDto.Position.ToUpper())) &&
                         (employeeDto.Name == null || employee.FirstName.ToUpper().Contains(employeeDto.Name.ToUpper()))
                         orderby employee.Id descending
                         select new EmployeeDto
                         {
                             Id = employee.Id,
                             Name = employee.FirstName + " " + employee.LastName,
                             Email = employee.Email,
                             Position = employee.Position,
                             BirthDate = employee.BirthDate,
                             DepartmentId = employee.DepartmentId,
                             DepartmentName = Departments.Name,
                             Salary = employee.Salary,
                             ManagerId = employee.ManagerId,
                             ManagerName = manager.FirstName
                         };

            return Ok(result);
        }


        [HttpGet("GetById/{id}")] // Route parameter --> part of url & u can use it when you have one parameter & the value required
        public IActionResult GetById(long id)
        {
            //var result = employees.Where(x => x.Id == id); // return [{ }] array
            //var result = employees.First(x => x.Id == id); // Invaild Exepction When enter id does not exist
            //var result = employees.SingleOrDefault(x => x.Id == id); // Invaild Exepction When there are two id's same
            //var result = employees.FirstOrDefault(x => x.Id == id); // return only one record (best option)

            if (id <= 0)
            {
                return BadRequest("Id Value Is Invalid!");
            }

            var result = _dbContext.Employees.Select(x => new EmployeeDto
            {
                Id = x.Id,
                Name = x.FirstName + " " + x.LastName,
                Email = x.Email,
                Position = x.Position,
                BirthDate = x.BirthDate,
                Salary = x.Salary,
                ManagerId = x.ManagerId,
                ManagerName = "",
                DepartmentId = x.DepartmentId,
                DepartmentName = ""
            }).FirstOrDefault(x => x.Id == id);

            if (result == null)
            {
                return NotFound("Employee Not Found!");
            }

            return Ok(result);
        }

        [HttpPost("Add")]

        public IActionResult Add([FromBody] SaveEmployeeDto employeeDto)
        {
            // employees.Add(employeeDto) :D you can't add something in model from dto 
            var employee = new Employee()
            {
                Id = 0, //(employees.LastOrDefault()?.Id ?? 0) + 1,
                FirstName = employeeDto.FirstName,
                LastName = employeeDto.LastName,
                Email = employeeDto.Email,
                BirthDate = (DateTime)employeeDto.BirthDate,
                Position = employeeDto.Position,
                Salary = employeeDto.Salary,
                DepartmentId = employeeDto.DepartmentId,
                ManagerId = employeeDto.ManagerId
            };
            _dbContext.Employees.Add(employee);
            _dbContext.SaveChanges(); // Commit: To Save Employee 

            return Ok();
        }

        [HttpPut("Update")]

        public IActionResult Update([FromBody] SaveEmployeeDto employeeDto)
        {
            var employee = _dbContext.Employees.FirstOrDefault(x => x.Id == employeeDto.Id);

            if (employee == null)
            {
                return NotFound("Employee Does Not Exist!");
            }

            employee.FirstName = employeeDto.FirstName;
            employee.LastName = employeeDto.LastName;
            employee.Email = employeeDto.Email;
            employee.BirthDate = (DateTime)employeeDto.BirthDate;
            employee.Position = employeeDto.Position;
            employee.Salary = employeeDto.Salary;
            employee.DepartmentId = employeeDto.DepartmentId;
            employee.ManagerId = employeeDto.ManagerId;
            _dbContext.SaveChanges();

            return Ok();

        }

        /* [HttpGet("Test")]
         public IActionResult Test()
         {
             var data = employees.LastOrDefault().Id + 1; // if no employees in the list it will Give you an Null Ref Execption

             return Ok(data);
         }
         */

        [HttpDelete("Delete/{id}")]
        public IActionResult Delete([FromQuery] long id)
        {
            var employee = _dbContext.Employees.FirstOrDefault(x => x.Id == id);

            if (employee == null)
            {
                return NotFound("Employee Does not exist!");
            }

            _dbContext.Employees.Remove(employee);
            _dbContext.SaveChanges();
            return Ok();
        }

        // long, int, string, bool ,etc... --> simple data type // Query Parameter (By Default)
        // Dtos, Model, Objects --> complex data type // Request Body (By Default)

        // Http Get: Cannot Use Body Request [FromBody], We can only Use Query Parameters [FromQuery]
        // Http Post/Put: Can Use Both Body Request [FromBody] & Query Parameter [FromQuery] but we will only use [FromBody]
        // Http Delete: Can Use Both Body Request [FromBody] & Query Parameter [FromQuery] but we will only use [FromQuery]

        // you cannot have more than one parameter of type --> [FromBody]
        // you can have more than one parameter of type --> [FromQuery]

    }
}
