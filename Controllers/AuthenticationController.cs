using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using CourseManagement.Models;
using CourseManagement.Services;
using AutoMapper;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CourseManagement.Controllers
{
    [ApiController]
    [Route("/")]
    public class AuthenticationController : ControllerBase
    {
        private readonly ILogger<AuthenticationController> _logger;

        private readonly AppDbContext _context;

        private readonly IMapper _mapper;

        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly TokenService _tokenService;

        public AuthenticationController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, TokenService tokenService, ILogger<AuthenticationController> logger)
        {
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
            _tokenService = tokenService;
        }

        [HttpPost("/register")]
        public async Task<IActionResult> Register([FromBody] RegisterUser registerUser, string role)
        {
            if (await _userManager.FindByEmailAsync(registerUser.Email) != null)
            {
                return BadRequest($"User With Email: {registerUser.Email} already exists!");
            }

            IdentityUser newUser = new()
            {
                Email = registerUser.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = registerUser.Name,
            };

            if (await _roleManager.RoleExistsAsync(role))
            {
                var registerResult = await _userManager.CreateAsync(newUser, registerUser.Password);
                if (!registerResult.Succeeded)
                {
                    return BadRequest("User failed to create!");
                }
                await _userManager.AddToRoleAsync(newUser, role);
                return Ok("User created successfully!");
            }
            else
            {
                return BadRequest($"Role: {role} not exists!");
            }
        }

        [HttpPost("/login")]
        public async Task<IActionResult> Login([FromBody] LoginUser loginUser)
        {
            var user = await _userManager.FindByNameAsync(loginUser.Username);
            if (user == null)
            {
                return BadRequest($"User with name: {loginUser.Username} not exists!");
            }
    
            if (!await _userManager.CheckPasswordAsync(user, loginUser.Password))
            {
                return Unauthorized("Username and password is incorrect!");
            }

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, loginUser.Username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var userRoles = await _userManager.GetRolesAsync(user);
            foreach(var role in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var jwtToken = _tokenService.GetToken(authClaims);
            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                expiration = jwtToken.ValidTo
            });
        }
    }
}