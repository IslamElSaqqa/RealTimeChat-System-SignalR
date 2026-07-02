using AdvancedChat.API.Services;
using API.DTOs;
using API.Models;
using API.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JwtService _jwtService;
        public AuthController(UserManager<ApplicationUser> userManager, JwtService jwtService)
        {
            _userManager = userManager;
            _jwtService = jwtService;
        }

        // Register
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto model) 
        {
            if (!ModelState.IsValid)
                return BadRequest(new AuthResponseDto { Success = false, Message = "Invalid input" });

            var existingUser = await _userManager.FindByEmailAsync(model.Email);

            if (existingUser != null)
            {
                return BadRequest(new AuthResponseDto { Success = false, Message = "Email already exists!" });
            }

            var user = new ApplicationUser 
            {
                FullName = model.FullName,
                UserName = model.Email,
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return BadRequest(new AuthResponseDto { Success = false, Message = $"Registration failed: {errors}" });
            }

            // Generate token immediately after registration
            var token = _jwtService.GenerateToken(user);

            return Ok(new AuthResponseDto 
            { 
                Success = true, 
                Token = token, 
                Message = "User registered successfully!",
                User = new UserDto { Id = user.Id, Email = user.Email, FullName = user.FullName }
            });
        }

        // Login
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new AuthResponseDto { Success = false, Message = "Invalid input" });

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return Unauthorized(new AuthResponseDto { Success = false, Message = "Incorrect email!" });
            }

            var validPassword = await _userManager.CheckPasswordAsync(user, model.Password);

            if (!validPassword)
            {
                return Unauthorized(new AuthResponseDto { Success = false, Message = "Incorrect password!" });
            }

            var token = _jwtService.GenerateToken(user);

            return Ok(new AuthResponseDto 
            { 
                Success = true, 
                Token = token, 
                Message = "Successful login!",
                User = new UserDto { Id = user.Id, Email = user.Email, FullName = user.FullName }
            });
        }
    }
}
