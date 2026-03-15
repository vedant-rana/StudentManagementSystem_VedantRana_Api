using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.DBContext;
using StudentManagementSystem.DTOs;
using StudentManagementSystem.Interfaces;

namespace StudentManagementSystem.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly AppDbContext _context;

        public StudentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Student>> GetAllAsync()
        {
            return await _context.Students
                .Include(s => s.Classes)
                .ToListAsync();
        }

        public async Task<(List<Student> Students, int TotalCount)> GetAllWithFilterAsync(StudentQueryParameters queryParams)
        {
            var query = _context.Students.Include(s => s.Classes).AsQueryable();

            if (!string.IsNullOrWhiteSpace(queryParams.Search))
            {
                var searchLower = queryParams.Search.ToLower();
                query = query.Where(s =>
                    s.FirstName.ToLower().Contains(searchLower) ||
                    s.LastName.ToLower().Contains(searchLower) ||
                    s.EmailId.ToLower().Contains(searchLower) ||
                    s.PhoneNumber.Contains(searchLower));
            }

            var totalCount = await query.CountAsync();

            query = ApplySorting(query, queryParams.SortField, queryParams.SortOrder);

            var students = await query
                .Skip((queryParams.Page - 1) * queryParams.PageSize)
                .Take(queryParams.PageSize)
                .ToListAsync();

            return (students, totalCount);
        }

        private IQueryable<Student> ApplySorting(IQueryable<Student> query, string sortField, string sortOrder)
        {
            var isAscending = sortOrder.ToLower() == "asc";

            return sortField.ToLower() switch
            {
                "firstname" => isAscending ? query.OrderBy(s => s.FirstName) : query.OrderByDescending(s => s.FirstName),
                "lastname" => isAscending ? query.OrderBy(s => s.LastName) : query.OrderByDescending(s => s.LastName),
                "email" or "emailid" => isAscending ? query.OrderBy(s => s.EmailId) : query.OrderByDescending(s => s.EmailId),
                "phonenumber" => isAscending ? query.OrderBy(s => s.PhoneNumber) : query.OrderByDescending(s => s.PhoneNumber),
                "createdat" => isAscending ? query.OrderBy(s => s.CreatedAt) : query.OrderByDescending(s => s.CreatedAt),
                _ => isAscending ? query.OrderBy(s => s.FirstName) : query.OrderByDescending(s => s.FirstName)
            };
        }

        public async Task<Student?> GetByIdAsync(int id)
        {
            return await _context.Students
                .Include(s => s.Classes)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<Student?> GetByEmailAsync(string email)
        {
            return await _context.Students
                .FirstOrDefaultAsync(s => s.EmailId == email);
        }

        public async Task<Student?> GetByPhoneNumberAsync(string phoneNumber)
        {
            return await _context.Students
                .FirstOrDefaultAsync(s => s.PhoneNumber == phoneNumber);
        }

        public async Task AddAsync(Student student)
        {
            await _context.Students.AddAsync(student);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Student student)
        {
            _context.Students.Update(student);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student != null)
            {
                _context.Students.Remove(student);
                await _context.SaveChangesAsync();
            }
        }
    }
}
