namespace Kemar.HRM.Model.Filter
{
    public class StudentFilter
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public bool? IsActive { get; set; }

        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;

        public string? SortBy { get; set; } = "StudentId";
        public bool SortDesc { get; set; } = false;
    }
}
