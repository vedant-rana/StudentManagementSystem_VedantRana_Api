using StudentManagementSystem.DBContext;
using StudentManagementSystem.DTOs;

namespace StudentManagementSystem.Interfaces
{
    public interface IStudentRepository
    {
        Task<IEnumerable<Student>> GetAllAsync();
        Task<(List<Student> Students, int TotalCount)> GetAllWithFilterAsync(StudentQueryParameters queryParams);
        Task<Student?> GetByIdAsync(int id);
        Task<Student?> GetByEmailAsync(string email);
        Task<Student?> GetByPhoneNumberAsync(string phoneNumber);
        Task AddAsync(Student student);
        Task UpdateAsync(Student student);
        Task DeleteAsync(int id);
    }
}
