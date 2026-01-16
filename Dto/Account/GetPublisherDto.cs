using News_App.Dto.newsPage;

namespace News_App.Dto.Account
{
    public class GetPublisherDto
    {
        public string ?Id { get; set; }
        public string ?Name { get; set; }
        public string? Email { get; set; }
        public List<newspageDto2> ? newspages { get; set; } = new List<newspageDto2>();
    }
}
