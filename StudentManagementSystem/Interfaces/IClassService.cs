using StudentManagementSystem.DTOs;

namespace StudentManagementSystem.Interfaces
{
    public interface IClassService
    {
        Task<IEnumerable<ClassDto>> GetAllClassesAsync();
        Task<ClassDto?> GetClassByIdAsync(int id);
        Task<ClassImportResultDto> ImportClassesAsync(IFormFile file);
    }
}
