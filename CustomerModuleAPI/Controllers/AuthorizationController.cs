using CustomerModuleAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CustomerModuleAPI.Controllers
{
    public class AuthorizationController : Controller
    {
        private readonly IConfiguration _iconfiguration;
        private readonly CustomerModuleContext _context;

        public AuthorizationController(CustomerModuleContext context, IConfiguration configuration)
        {
            _context = context;
            _iconfiguration = configuration;
        }


        [HttpPost("Authorization")]
        public async Task<IActionResult> Post(string EmailID)
        {
            if (EmailID != null)
            {
                var customerdata = await GetCustomer(EmailID);
                var jwt = _iconfiguration.GetSection("Jwt").Get<Jwt>();
                if (EmailID != null)
                {
                    var claims = new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, jwt.Subject),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim ("FirstName", customerdata.FirstName),
                        new Claim ("EmailID", customerdata.EmailID),
                        new Claim ("MobileNo", customerdata.MobileNo)
                    };
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.key));
                    var signin = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                        jwt.Issuer,
                        jwt.Audience,
                        claims,
                        expires: DateTime.Now.AddMinutes(20),
                        signingCredentials: signin
                        );
                    return Ok(new JwtSecurityTokenHandler().WriteToken(token));

                }
                else
                {
                    return BadRequest("Invalid Credentials");
                }

            }
            else
            {
                return BadRequest("Invalid Credentials");
            }
        }


        [HttpGet]
        public async Task <CustomerInfo> GetCustomer (string EmailID)
        {
            return await _context.CustomerInfos.FirstOrDefaultAsync(u => u.EmailID == EmailID);
        }
    }
}
