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
    public class ClassesController : ControllerBase
    {
        private readonly IClassService _classService;

        public ClassesController(IClassService classService)
        {
            _classService = classService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<ClassDto>>>> GetAllClasses()
        {
            var classes = await _classService.GetAllClassesAsync();
            return Ok(new ApiResponse<IEnumerable<ClassDto>>(true, "Classes retrieved successfully", classes));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<ClassDto>>> GetClassById(int id)
        {
            var classEntity = await _classService.GetClassByIdAsync(id);
            return Ok(new ApiResponse<ClassDto>(true, "Class retrieved successfully", classEntity));
        }

        [HttpPost("import")]
        public async Task<ActionResult<ApiResponse<ClassImportResultDto>>> ImportClasses(IFormFile file)
        {
            var result = await _classService.ImportClassesAsync(file);
            return Ok(new ApiResponse<ClassImportResultDto>(true, "Classes imported successfully", result));
        }
    }
}
