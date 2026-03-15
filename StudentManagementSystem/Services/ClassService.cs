using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Http;
using StudentManagementSystem.DBContext;
using StudentManagementSystem.DTOs;
using StudentManagementSystem.Interfaces;

namespace StudentManagementSystem.Services
{
    public class ClassService : IClassService
    {
        private readonly IClassRepository _classRepository;
        private const long MaxFileSizeInBytes = 5 * 1024 * 1024;

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

        public async Task<ClassImportResultDto> ImportClassesAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("File is required");
            }

            if (file.Length > MaxFileSizeInBytes)
            {
                throw new ArgumentException("File size must not exceed 5 MB");
            }

            var extension = Path.GetExtension(file.FileName)?.ToLower();
            if (extension != ".csv")
            {
                throw new ArgumentException("File must be a CSV file");
            }

            var classImports = new List<ClassImportDto>();
            var errors = new List<string>();

            try
            {
                using (var stream = new StreamReader(file.OpenReadStream()))
                using (var csv = new CsvReader(stream, new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HeaderValidated = null,
                    MissingFieldFound = null
                }))
                {
                    var records = csv.GetRecords<ClassImportDto>();
                    classImports = records.ToList();
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Error parsing CSV file: {ex.Message}");
            }

            if (!classImports.Any())
            {
                throw new ArgumentException("CSV file is empty or contains no valid data");
            }

            for (int i = 0; i < classImports.Count; i++)
            {
                var row = classImports[i];
                var rowNumber = i + 2;

                if (string.IsNullOrWhiteSpace(row.Name))
                {
                    errors.Add($"Row {rowNumber}: Name is required");
                }

                if (!string.IsNullOrEmpty(row.Description) && row.Description.Length > 100)
                {
                    errors.Add($"Row {rowNumber}: Description cannot exceed 100 characters");
                }
            }

            if (errors.Any())
            {
                throw new ArgumentException(string.Join("; ", errors));
            }

            var classNames = classImports.Select(c => c.Name).ToList();
            var existingClassNames = await _classRepository.GetExistingClassNamesAsync(classNames);
            var existingNamesLower = existingClassNames.Select(n => n.ToLower()).ToHashSet();

            var newClasses = classImports
                .Where(c => !existingNamesLower.Contains(c.Name.ToLower()))
                .Select(c => new Class
                {
                    Name = c.Name,
                    Description = c.Description
                })
                .ToList();

            if (newClasses.Any())
            {
                await _classRepository.AddRangeAsync(newClasses);
            }

            return new ClassImportResultDto
            {
                TotalRows = classImports.Count,
                Inserted = newClasses.Count,
                Duplicates = classImports.Count - newClasses.Count
            };
        }
    }
}
