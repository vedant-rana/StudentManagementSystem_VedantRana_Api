using StudentManagementSystem.DBContext;

namespace StudentManagementSystem.Interfaces
{
    public interface IClassRepository
    {
        Task<IEnumerable<Class>> GetAllAsync();
        Task<Class?> GetByIdAsync(int id);
        Task<List<Class>> GetByIdsAsync(List<int> ids);
        Task AddRangeAsync(List<Class> classes);
    }
}
