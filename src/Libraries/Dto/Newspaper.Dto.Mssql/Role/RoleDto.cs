namespace Newspaper.Dto.Mssql.Role
{
    public class RoleDto : BaseDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public int UserCount { get; set; }
    }
}
