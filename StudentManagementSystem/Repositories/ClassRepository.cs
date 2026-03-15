using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.DBContext;
using StudentManagementSystem.Interfaces;

namespace StudentManagementSystem.Repositories
{
    public class ClassRepository : IClassRepository
    {
        private readonly AppDbContext _context;

        public ClassRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Class>> GetAllAsync()
        {
            return await _context.Classes.ToListAsync();
        }

        public async Task<Class?> GetByIdAsync(int id)
        {
            return await _context.Classes.FindAsync(id);
        }

        public async Task<List<Class>> GetByIdsAsync(List<int> ids)
        {
            return await _context.Classes
                .Where(c => ids.Contains(c.Id))
                .ToListAsync();
        }

        public async Task<bool> ExistsByNameAsync(string name)
        {
            return await _context.Classes
                .AnyAsync(c => c.Name.ToLower() == name.ToLower());
        }

        public async Task<List<string>> GetExistingClassNamesAsync(List<string> names)
        {
            var lowerNames = names.Select(n => n.ToLower()).ToList();
            return await _context.Classes
                .Where(c => lowerNames.Contains(c.Name.ToLower()))
                .Select(c => c.Name)
                .ToListAsync();
        }

        public async Task AddRangeAsync(List<Class> classes)
        {
            await _context.Classes.AddRangeAsync(classes);
            await _context.SaveChangesAsync();
        }
    }
}
