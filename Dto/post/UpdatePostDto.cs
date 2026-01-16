namespace News_App.Dto.post
{
    public class UpdatePostDto
    {
        public string? Title { get; set; }
        public string? Post1 { get; set; }
        public IFormFileCollection Images { get; set; }
    }
}