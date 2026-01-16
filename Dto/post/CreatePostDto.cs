namespace News_App.Dto.post
{
    public class CreatePostDto
    {
        public string title { get; set; }
        public string Post1 { get; set; }
        public string PublisherId { get; set; }
        public IFormFileCollection Images { get; set; }
        public int NewsPageId { get; set; }
        public int CategoryId { get; set; }

    }
}
