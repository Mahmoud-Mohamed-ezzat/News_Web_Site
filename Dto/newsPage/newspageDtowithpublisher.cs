using News_App.Models;

namespace News_App.Dto.newsPage
{
    public class newspageDtowithpublisher
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public bool? Iscreated { get; set; }
        public string? AdminId { get; set; }
        public virtual ICollection<User>?Publishers { get; set; } = new List<User>();

    }
}
