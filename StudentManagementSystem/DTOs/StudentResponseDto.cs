namespace StudentManagementSystem.DTOs
{
    public class StudentResponseDto
    {
        public int Id { get; set; }

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string PhoneNumber { get; set; } = null!;

        public string EmailId { get; set; } = null!;

        public DateTime? CreatedAt { get; set; }

        public List<ClassDto> Classes { get; set; } = new List<ClassDto>();
    }
}
