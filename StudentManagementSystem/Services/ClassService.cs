using StudentManagementSystem.DBContext;
using StudentManagementSystem.DTOs;
using StudentManagementSystem.Interfaces;

namespace StudentManagementSystem.Services
{
    public class ClassService : IClassService
    {
        private readonly IClassRepository _classRepository;

        public ClassService(IClassRepository classRepository)
        {
            _classRepository = classRepository;
        }

        public async Task<IEnumerable<ClassDto>> GetAllClassesAsync()
        {
            var classes = await _classRepository.GetAllAsync();
            return classes.Select(c => new ClassDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description
            });
        }

        public async Task<ClassDto?> GetClassByIdAsync(int id)
        {
            var classEntity = await _classRepository.GetByIdAsync(id);
            if (classEntity == null)
            {
                throw new KeyNotFoundException($"Class with ID {id} not found");
            }

            return new ClassDto
            {
                Id = classEntity.Id,
                Name = classEntity.Name,
                Description = classEntity.Description
            };
        }
    }
}
