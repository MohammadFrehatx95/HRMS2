using HRMS.DbContexts;
using HRMS.Models;
using HRMS2.Dtos.Employees;
using HRMS2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.ExceptionServices;
using System.Security.Claims;

namespace HRMS2.Controllers
{
    [Authorize]
    [Route("api/[controller]")]// Data Annotation
    [ApiController]// Data Annotation
    public class EmployeesController : ControllerBase
    {
        // Depndency Injuction
        private readonly HRMSContext _dbContext;
        public EmployeesController(HRMSContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Nuget Packeage

        // CRUD Operations 
        // C : Create
        // R : Read
        // U : Update
        // D : Delete

        // Read
        [HttpGet("GetByCriteria")] // Data Annotation : Method -> Api Endpoint
        public IActionResult GetByCriteria([FromQuery] SearchEmployeeDto employeeDto) // (?) --> Optional / Nullable
        {
            // From Token
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            try
            {
                var result = from employee in _dbContext.Employees
                             from department in _dbContext.Departments.Where(x => x.Id == employee.DepartmentId).DefaultIfEmpty() // Left Join
                             from manager in _dbContext.Employees.Where(x => x.Id == employee.ManagerId).DefaultIfEmpty() // Left Join
                             from lookup in _dbContext.Lookups.Where(x => x.Id == employee.PositionId)
                             where
                             (employeeDto.PositionId == null || employee.PositionId == employeeDto.PositionId) &&
                             (employeeDto.Name == null || employee.FirstName.ToUpper().Contains(employeeDto.Name.ToUpper()))
                             orderby employee.Id descending
                             select new EmployeeDto
                             {
                                 Id = employee.Id,
                                 Name = employee.FirstName + " " + employee.LastName,
                                 PositionId = employee.PositionId,
                                 PositionName = lookup.Name,
                                 BirthDate = employee.BirthDate,
                                 Email = employee.Email,
                                 Salary = employee.Salary,
                                 DepartmentId = employee.DepartmentId,
                                 DepartmentName = department.Name,
                                 ManagerId = employee.ManagerId,
                                 ManagerName = manager.FirstName,
                                 UserId = employee.UserId
                             };

                if(role?.ToUpper() != "Admin" && role?.ToUpper() != "HR")
                {
                   result = result.Where(x => x.UserId == long.Parse(userId));
                }


                return Ok(result);

                //return Ok(new { Name = "Ahmad", Age = 26});// 200
                //return NotFound("No Data Found");// 404
                //return BadRequest("Data Missing"); // 400
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("GetById/{id}")] // Route Parameter
        public IActionResult GetById(long id)
        {
            // From Token
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            try
            {
                if (id == 0)
                {
                    return BadRequest("Id Value Is Invalid");
                }
                var result = _dbContext.Employees.Select(x => new EmployeeDto // Projection
                {
                    Id = x.Id,
                    Name = x.FirstName + " " + x.LastName,
                    PositionId = x.PositionId,
                    PositionName = x.lookup.Name,
                    BirthDate = x.BirthDate,
                    Email = x.Email,
                    Salary = x.Salary,
                    DepartmentId = x.DepartmentId,
                    DepartmentName = x.Department.Name,
                    ManagerId = x.ManagerId,
                    ManagerName = x.Manager.FirstName,
                    UserId = x.UserId
                }).FirstOrDefault(x => x.Id == id);

                if (role?.ToUpper() != "Admin" && role?.ToUpper() != "HR")
                {
                   if(result.UserId != long.Parse(userId))
                   {
                        return Forbid("You Are Not Authorized To Access This Employee Data"); // 403
                   }
                }


                /*
                 var result = _dbContext.Employees.Include(x => x.Lookup).Include(x => x.Manager).ThenInclude(x => x.Lookup) // Eager Loading
                    .FirstOrDefault(x => x.Id == id); 
                */


                // Projection --> Select
                // Eager Loading --> Include
                // Lazy Loading --> ??

                if (result == null)
                {
                    return NotFound("Employee Not Found");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "HR,Admin")]
        [HttpPost("Add")] // Create
        public IActionResult Add([FromBody] SaveEmployeeDto employeeDto)
        {
            var user = new User()
            {
                Id = 0,
                Username = $"{employeeDto.FirstName}_{employeeDto.LastName}_HRMS",
                HashedPassword = BCrypt.Net.BCrypt.HashPassword($"{employeeDto.FirstName}@123"),
                IsAdmin = false
            };

            var IsUsernameExist = _dbContext.Users.Any(x => x.Username.ToUpper() == user.Username.ToUpper());
            
            if (IsUsernameExist)
            {
                return BadRequest("Username Already Exists, Please Choose Another one");
            }

            _dbContext.Users.Add(user);

            try
            {
                var employee = new Employee()
                {
                    Id = 0, //(employees.LastOrDefault()?.Id ?? 0) + 1,
                    FirstName = employeeDto.FirstName,
                    LastName = employeeDto.LastName,
                    Email = employeeDto.Email,
                    BirthDate = employeeDto.BirthDate,
                    PositionId = employeeDto.PositionId,
                    Salary = employeeDto.Salary,
                    DepartmentId = employeeDto.DepartmentId,
                    ManagerId = employeeDto.ManagerId,
                    //UserId = user.Id
                    User = user // EF understand (Navigation Prop) is better than save changes two times.
                };
                _dbContext.Employees.Add(employee);
                _dbContext.SaveChanges(); // Commit

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [Authorize(Roles = "HR,Admin")]
        [HttpPut("Update")] // Update
        public IActionResult Update([FromBody] SaveEmployeeDto employeeDto)
        {
            try
            {
                var employee = _dbContext.Employees.FirstOrDefault(x => x.Id == employeeDto.Id);


                if (employee == null)
                {
                    return NotFound("Employee Does Not Exist");
                }


                employee.FirstName = employeeDto.FirstName;
                employee.LastName = employeeDto.LastName;
                employee.Email = employeeDto.Email;
                employee.BirthDate = employeeDto.BirthDate;
                employee.PositionId = employeeDto.PositionId;
                employee.Salary = employeeDto.Salary;
                employee.DepartmentId = employeeDto.DepartmentId;
                employee.ManagerId = employeeDto.ManagerId;
                _dbContext.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [Authorize(Roles = "HR,Admin")]
        [HttpDelete("Delete/{id}")] // Delete
        public IActionResult Delete(long id)
        {
            try
            {
                var employee = _dbContext.Employees.FirstOrDefault(x => x.Id == id);

                if (employee == null)
                {
                    return NotFound("Employee Does Not Exist"); // 404
                }

                _dbContext.Employees.Remove(employee);
                _dbContext.SaveChanges();
                return Ok();
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

    }



    // Simple Data Types --> string, int, double, long.... // Query Parameter (By Default) 
    // Complix Data Types --> Objects, Dtos, Model... // Body Request (By Defualt)

    // Http Get : Can Not Use Body Request [FromBody], We Can Only Use Query Parameters [FromQuery]
    // Http Post/Put : Can Use Both Body Request [FromBody] And Query Parameter [FromQuery], But We Will Only Use [FromBody]
    // Http Delete : Can Use Both Body Request [FromBody] And Query Parameter [FromQuery], But We Will Only Use [FromQuery]

    // Method Cannot Use Multiple Parameters Of Type [FromBody]
    // Method Can Use Multiple Parameters Of Type [FromQuery]

}

