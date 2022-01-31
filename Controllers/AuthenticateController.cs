
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using dotnet_api_project.IdentityAuth;
using Microsoft.AspNetCore.Authorization;
using dotnet_api_project.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Transactions;

namespace dotnet_api_project.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class AuthenticateController: ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthenticateController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest model)
        {
            Console.WriteLine("username value is "+ model.Username);
            Console.WriteLine("email value is "+ model.Email);
            Console.WriteLine("password value is "+ model.Password);

            // Console.WriteLine("Any String");

            var userExist = await _userManager.FindByNameAsync(model.Username);
            if(userExist != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response{Status = "Error", Message = "User already exist"});
            
            ApplicationUser user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };
           

            var resutSaveUser = await _userManager.CreateAsync(user, model.Password);
            
            if (!resutSaveUser.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response{ Status = "Error", Message = resutSaveUser.ToString()});

            var resultSaveUserRole = await _userManager.AddToRoleAsync(user, UserRoles.User);

             if (!resultSaveUserRole.Succeeded)
                 return StatusCode(StatusCodes.Status500InternalServerError, new Response{ Status = "Error", Message = resultSaveUserRole.ToString()});

            return Ok(new Response{ Status = "Success", Message ="User created successfully"});
        }

        
        [HttpPost]
        [Route("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterRequest model)
        {
            using(var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var userExist = await _userManager.FindByNameAsync(model.Username);
                    if(userExist != null)
                        return StatusCode(StatusCodes.Status500InternalServerError, new Response{Status = "Error", Message = "User already exist"});
                    
                    ApplicationUser user = new()
                    {
                        Email = model.Email,
                        SecurityStamp = Guid.NewGuid().ToString(),
                        UserName = model.Username
                    };

                    var result = await _userManager.CreateAsync(user, model.Password);
                    var resultSaveUserRole = await _userManager.AddToRoleAsync(user, UserRoles.Admin);

                }
                catch (System.Exception ex)
                {
                    scope.Dispose();
                   return StatusCode(StatusCodes.Status500InternalServerError, new Response{ Status = "Error", Message = ex.Message.ToString()});
                }
                scope.Complete();
            }

            return Ok(new Response{ Status="Success", Message="User created successfully!" }); 
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if(user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                var authClaim = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                foreach (var userRole in userRoles)
                {
                    authClaim.Add(new Claim(ClaimTypes.Role, userRole));
                }
                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"].ToString()));
                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"].ToString(),
                    audience: _configuration["JWT:ValidAudience"].ToString(),
                    expires: DateTime.Now.AddMinutes(10),
                    claims: authClaim,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

                return Ok(new {token = new JwtSecurityTokenHandler().WriteToken(token), expiration = token.ValidTo});
            }

            return Unauthorized();
        }
    }
}