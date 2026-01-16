using News_App.Dto.newsPage;

namespace News_App.Dto.Account
{
    public class CreatePageandSignupFoRAdminOfWebDto
    {
        public SignupFoRAdminOfWebDto user { get; set; }
        public createNewspagewithadminIdDTO page { get; set; } = new createNewspagewithadminIdDTO();
    }
}
