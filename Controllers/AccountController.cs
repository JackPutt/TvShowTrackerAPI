using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TvShowTrackerApi.Models;

namespace TvShowTrackerApi.Controllers
{
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public AccountController(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        //POST: Register a new user to the app
        [HttpPost]
        [Route("api/register")]
        public async Task<IActionResult> Register([FromBody] Register register)
        {
            //Check to see if user already exists
            var userExists = await _userManager.FindByNameAsync(register.Username);
            if (userExists != null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists." });
            }

            //Create new application user
            ApplicationUser user = new ApplicationUser()
            {
                Email = register.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = register.Username
            };

            var result = await _userManager.CreateAsync(user, register.Password);

            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed." });
            }
            else
            {
                return StatusCode(StatusCodes.Status201Created, new Response { Status = "Created", Message = "User created." });
            }
        }

        [HttpPost]
        [Route("api/login")]
        public async Task<IActionResult> Login([FromBody] Login login)
        {
            //Find user
            var user = await _userManager.FindByNameAsync(login.Username);

            if (user != null)
            {
                //Check password matches
                if (await _userManager.CheckPasswordAsync(user, login.Password))
                {
                    //Generate metadata for token
                    var authClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                    };

                    var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                    //Generate token
                    var token = new JwtSecurityToken(
                        issuer: _configuration["JWT:ValidIssuer"],
                        audience: _configuration["JWT:ValidAudience"],
                        expires: DateTime.Now.AddHours(1),
                        claims: authClaims,
                        signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                        );

                    return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token), expiration = token.ValidTo, userId = user.Id });
                }
                else
                {
                    return StatusCode(StatusCodes.Status401Unauthorized, new Response { Status = "Unauthorised", Message = "Username and/or password is wrong." });
                }
            }
            else
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new Response { Status = "Unauthorised", Message = "User does not exist." });
            }
        }
    }
}
