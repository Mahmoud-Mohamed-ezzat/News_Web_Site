namespace News_App.Dto.newsPage
{
    public class createnewspageforAdminOfPage
    { // if admin need to make his page
        public string? Name { get; set; }
        public bool? Iscreated { get; set; } = true;
        public string? AdminId { get; set; }
    }
}
