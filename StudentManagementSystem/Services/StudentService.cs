using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.DBContext;
using StudentManagementSystem.DTOs;
using StudentManagementSystem.Interfaces;

namespace StudentManagementSystem.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IClassRepository _classRepository;

        public StudentService(IStudentRepository studentRepository, IClassRepository classRepository)
        {
            _studentRepository = studentRepository;
            _classRepository = classRepository;
        }

        public async Task<IEnumerable<StudentResponseDto>> GetAllStudentsAsync()
        {
            var students = await _studentRepository.GetAllAsync();
            return students.Select(MapToResponseDto);
        }

        public async Task<StudentResponseDto?> GetStudentByIdAsync(int id)
        {
            var student = await _studentRepository.GetByIdAsync(id);
            if (student == null)
            {
                throw new KeyNotFoundException($"Student with ID {id} not found");
            }
            return MapToResponseDto(student);
        }

        public async Task<StudentResponseDto> CreateStudentAsync(StudentCreateDto studentDto)
        {
            var existingEmail = await _studentRepository.GetByEmailAsync(studentDto.EmailId);
            if (existingEmail != null)
            {
                throw new InvalidOperationException("Email already exists");
            }

            var existingPhone = await _studentRepository.GetByPhoneNumberAsync(studentDto.PhoneNumber);
            if (existingPhone != null)
            {
                throw new InvalidOperationException("Phone number already exists");
            }

            var classes = new List<Class>();
            if (studentDto.ClassIds != null && studentDto.ClassIds.Any())
            {
                classes = await _classRepository.GetByIdsAsync(studentDto.ClassIds);
                if (classes.Count != studentDto.ClassIds.Count)
                {
                    throw new ArgumentException("One or more class IDs are invalid");
                }
            }

            var student = new Student
            {
                FirstName = studentDto.FirstName,
                LastName = studentDto.LastName,
                PhoneNumber = studentDto.PhoneNumber,
                EmailId = studentDto.EmailId,
                Classes = classes
            };

            await _studentRepository.AddAsync(student);
            return MapToResponseDto(student);
        }

        public async Task<StudentResponseDto> UpdateStudentAsync(int id, StudentUpdateDto studentDto)
        {
            var student = await _studentRepository.GetByIdAsync(id);
            if (student == null)
            {
                throw new KeyNotFoundException($"Student with ID {id} not found");
            }

            var existingEmail = await _studentRepository.GetByEmailAsync(studentDto.EmailId);
            if (existingEmail != null && existingEmail.Id != id)
            {
                throw new InvalidOperationException("Email already exists");
            }

            var existingPhone = await _studentRepository.GetByPhoneNumberAsync(studentDto.PhoneNumber);
            if (existingPhone != null && existingPhone.Id != id)
            {
                throw new InvalidOperationException("Phone number already exists");
            }

            var classes = new List<Class>();
            if (studentDto.ClassIds != null && studentDto.ClassIds.Any())
            {
                classes = await _classRepository.GetByIdsAsync(studentDto.ClassIds);
                if (classes.Count != studentDto.ClassIds.Count)
                {
                    throw new ArgumentException("One or more class IDs are invalid");
                }
            }

            student.FirstName = studentDto.FirstName;
            student.LastName = studentDto.LastName;
            student.PhoneNumber = studentDto.PhoneNumber;
            student.EmailId = studentDto.EmailId;
            student.Classes = classes;

            await _studentRepository.UpdateAsync(student);
            return MapToResponseDto(student);
        }

        public async Task DeleteStudentAsync(int id)
        {
            var student = await _studentRepository.GetByIdAsync(id);
            if (student == null)
            {
                throw new KeyNotFoundException($"Student with ID {id} not found");
            }

            await _studentRepository.DeleteAsync(id);
        }

        private StudentResponseDto MapToResponseDto(Student student)
        {
            return new StudentResponseDto
            {
                Id = student.Id,
                FirstName = student.FirstName,
                LastName = student.LastName,
                PhoneNumber = student.PhoneNumber,
                EmailId = student.EmailId,
                CreatedAt = student.CreatedAt,
                Classes = student.Classes.Select(c => new ClassDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description
                }).ToList()
            };
        }
    }
}
