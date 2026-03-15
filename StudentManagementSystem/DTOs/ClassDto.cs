using System.ComponentModel.DataAnnotations;

namespace StudentManagementSystem.DTOs
{
    public class ClassDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Class name is required")]
        [MaxLength(100, ErrorMessage = "Class name cannot exceed 100 characters")]
        public string Name { get; set; } = null!;

        [MaxLength(100, ErrorMessage = "Description cannot exceed 100 characters")]
        public string? Description { get; set; }
    }
}
