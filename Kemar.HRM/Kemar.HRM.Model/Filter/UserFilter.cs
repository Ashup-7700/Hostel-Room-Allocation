namespace Kemar.HRM.Model.Filter
{
    public class UserFilter
    {
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; }
        public bool? IsActive { get; set; }

        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;

        public string? SortBy { get; set; } = "UserId";
        public bool SortDesc { get; set; } = false;
    }
}
