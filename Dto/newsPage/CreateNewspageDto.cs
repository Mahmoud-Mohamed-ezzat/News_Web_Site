namespace News_App.Dto.newsPage
{
    public class CreateNewspageDto
    {
        public string? Name { get; set; }
        public bool? Iscreated { get; set; } = true;
        public string? AdminId { get; set; }
    }
}
