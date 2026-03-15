using System.ComponentModel.DataAnnotations;

namespace StudentManagementSystem.DTOs
{
    public class ClassImportDto
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } = null!;

        [MaxLength(100, ErrorMessage = "Description cannot exceed 100 characters")]
        public string? Description { get; set; }
    }
}
