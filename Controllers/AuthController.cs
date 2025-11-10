using HRMS.DbContexts;
using HRMS.Models;
using HRMS2.Dtos.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HRMS2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly HRMSContext _dbContext;
        public AuthController(HRMSContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost("Login")]

        public IActionResult Login([FromBody] LoginDto loginDto)
        {
            try
            {
                // Admin, admin , ADMIN //ADMIN == ADMIN
                var user = _dbContext.Users.FirstOrDefault(x => x.Username.ToUpper() == loginDto.Username.ToUpper());

                if (user == null)
                {
                    return BadRequest("Invalid username or password.");
                }

                // Admin@123 == $2a$11$FodwrXysOiJ9lFlf1PZGZOQZH1fvBzBivVnSewumv5QTqlDIXh1/e
                //if (loginDto.Password == user.HashedPassword)
                if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.HashedPassword))
                {
                    return BadRequest("Invalid Username Or Password");
                }

                var token = GenerateJwtToken(user);
                return Ok(token);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private string GenerateJwtToken(User user)
        {
            // User Info
            var claims = new List<Claim>();

            // Key -> Value
            // Id --> 1
            // Name --> Admin
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            claims.Add(new Claim(ClaimTypes.Name, user.Username));

            // Role --> HR , Manager , Developer , Admin
            if (user.IsAdmin)
            {
                claims.Add(new Claim(ClaimTypes.Role, "Admin"));
            }
            else
            {
                var employee = _dbContext.Employees.Include(x => x.lookup).FirstOrDefault(x => x.UserId == user.Id);
                claims.Add(new Claim(ClaimTypes.Role, employee.lookup.Name));
            }

            // Secret Key  = WHAFWEI#!@S!!112312WQEQW@RWQEQW432
            // Encoding.UTF8.GetBytes("WHAFWEI#!@S!!112312WQEQW@RWQEQW432") == [65, 45, 41,....]
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("WHAFWEI#!@S!!112312WQEQW@RWQEQW432"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256); // Signing the token 

            var tokenSettings = new JwtSecurityToken(
                  claims: claims, // User Info
                  signingCredentials: creds, // Encryption Settings | Secret Key
                  expires: DateTime.Now.AddDays(1) // When Does The Toke Expire
                );

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.WriteToken(tokenSettings);

            return token;
        }
    }
}
