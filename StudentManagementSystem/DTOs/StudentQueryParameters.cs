namespace StudentManagementSystem.DTOs
{
    public class StudentQueryParameters
    {
        public string? Search { get; set; }
        public string SortField { get; set; } = "firstName";
        public string SortOrder { get; set; } = "asc";
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
