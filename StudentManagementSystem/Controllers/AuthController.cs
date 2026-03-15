using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.DTOs;
using StudentManagementSystem.Models;
using StudentManagementSystem.Services;

namespace StudentManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly JwtService _jwtService;

        public AuthController(JwtService jwtService)
        {
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public IActionResult Login(LoginDto dto)
        {
            var response = new ApiResponse<object>();

            try
            {
                // Hardcoded user
                var hardcodedUser = new LoginDto
                {
                    Email = "admin@test.com",
                    Password = "admin123"
                };

                // Check credentials
                if (dto.Email != hardcodedUser.Email || dto.Password != hardcodedUser.Password)
                {
                    response.Success = false;
                    response.Message = "Invalid credentials";
                    response.Errors.Add("Email or password incorrect");

                    return Unauthorized(response);
                }

                // Generate JWT token
                var token = _jwtService.GenerateToken(hardcodedUser);

                response.Success = true;
                response.Message = "Login successful";
                response.Data = new { token };

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Login failed";
                response.Errors.Add(ex.Message);

                return StatusCode(500, response);
            }
        }
    }
}