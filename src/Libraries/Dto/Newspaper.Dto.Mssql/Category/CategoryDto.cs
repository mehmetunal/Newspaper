namespace Newspaper.Dto.Mssql.Category
{
    public class CategoryDto : BaseDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Slug { get; set; }
        public bool IsActive { get; set; }
        public int ArticleCount { get; set; }
    }
} 