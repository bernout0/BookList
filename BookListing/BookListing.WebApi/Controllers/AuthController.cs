using BookListing.Application.Common.Interfaces;
using BookListing.Domain.Identity;
using BookListing.Infrastructure.Helpers;
using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BookListing.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly AppSettings _appSettings;
        private readonly ICurrentUserService _currentUserService;

        public AuthController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            AppSettings appSettings,
           ICurrentUserService currentUserService )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _appSettings = appSettings;
            _currentUserService = currentUserService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null)
            {
                return BadRequest("Invalid username or password.");
            }

            var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, false, false);
            if (!result.Succeeded)
            {
                return BadRequest("Invalid username or password.");
            }

            // Usually, IdentityServer would handle this part automatically once you redirect to its endpoint.
            // Here, we're simulating a token creation for simplicity. In real-world scenarios, you'd 
            // interact with IdentityServer's token endpoint directly from the client after login success.
            var token = GenerateTokenForUser(user); // Define this function to generate JWT for the user

            return Ok(new { token });
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] LoginRequest model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user != null)
            {
                return BadRequest("Username Already Exists.");
            }
            var newUser = new User()
            {
                UserName = model.Username,
                Email = model.Username
            };


            var identity = await _userManager.CreateAsync(newUser, model.Password);

            var actualUser = await _userManager.FindByNameAsync(model.Username);

            // Usually, IdentityServer would handle this part automatically once you redirect to its endpoint.
            // Here, we're simulating a token creation for simplicity. In real-world scenarios, you'd 
            // interact with IdentityServer's token endpoint directly from the client after login success.
            var token = GenerateTokenForUser(actualUser); // Define this function to generate JWT for the user

            return Ok(new { token });
        }

        [HttpGet("token")]
        public async Task<IActionResult> Token()
        {
            var userId = _currentUserService.UserId;

            var user = await _userManager.FindByNameAsync(userId);
            if (user == null)
            {
                return BadRequest("Invalid user.");
            }
                       

            // Usually, IdentityServer would handle this part automatically once you redirect to its endpoint.
            // Here, we're simulating a token creation for simplicity. In real-world scenarios, you'd 
            // interact with IdentityServer's token endpoint directly from the client after login success.
            var token = GenerateTokenForUser(user); // Define this function to generate JWT for the user

            return Ok(new { token });
        }

        private string GenerateTokenForUser(IdentityUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName)
                // Add other claims as needed
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.Secret));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
