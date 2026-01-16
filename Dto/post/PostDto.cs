namespace News_App.Dto.post
{
    public class PostDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Post1 { get; set; }
        public List<string>? Images { get; set; }
        public string? PublisherId { get; set; }
        public string? PublisherName { get; set; }
        public int? CategoryId { get; set; }
        public string? Categoryname { get; set; }
        public int? NewspageId { get; set; }
        public string? NewsPageName { get; set; }
    }
}
