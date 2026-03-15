using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.DTOs;
using StudentManagementSystem.Interfaces;
using StudentManagementSystem.Models;

namespace StudentManagementSystem.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentsController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<PaginatedResult<StudentResponseDto>>>> GetAllStudents([FromQuery] StudentQueryParameters queryParams)
        {
            var result = await _studentService.GetAllStudentsWithFilterAsync(queryParams);
            return Ok(new ApiResponse<PaginatedResult<StudentResponseDto>>(true, "Students retrieved successfully", result));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<StudentResponseDto>>> GetStudentById(int id)
        {
            var student = await _studentService.GetStudentByIdAsync(id);
            return Ok(new ApiResponse<StudentResponseDto>(true, "Student retrieved successfully", student));
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<StudentResponseDto>>> CreateStudent([FromBody] StudentCreateDto studentDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Validation failed",
                    Errors = errors
                });
            }

            var student = await _studentService.CreateStudentAsync(studentDto);
            return Ok(new ApiResponse<StudentResponseDto>(true, "Student created successfully", student));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<StudentResponseDto>>> UpdateStudent(int id, [FromBody] StudentUpdateDto studentDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Validation failed",
                    Errors = errors
                });
            }

            var student = await _studentService.UpdateStudentAsync(id, studentDto);
            return Ok(new ApiResponse<StudentResponseDto>(true, "Student updated successfully", student));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteStudent(int id)
        {
            await _studentService.DeleteStudentAsync(id);
            return Ok(new ApiResponse<object>(true, "Student deleted successfully", null));
        }
    }
}
