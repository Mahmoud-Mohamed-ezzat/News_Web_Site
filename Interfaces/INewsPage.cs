using Microsoft.AspNetCore.Mvc;
using News_App.Dto.newsPage;
using News_App.Models;
using News_App.Dto.Account;
namespace News_App.Interfaces
{
    public interface INewsPage
    {
        public Task<List<newspageDto2>> GetPages();
        public Task<List<newspageDto>> GetPageswithids();
        public Task<List<newspageDtowithpublisher>> getpagesbyPublishers();
        public Task<newspageDto2> GetpageByid(int id);
        public Task<List<Newspage>> GetPagesOfAdmin(string AdminId);        
        public Task<Newspage> GetpagewithPublishersByid(int id);
        public Task<Newspage> GetpageByName(string name);
        public Task<List<NewsPageDto3>> GetPagesUnderRequest();
        public Task<Newspage> AddPage(CreateNewspageDto model);
        public Task<Newspage> AddPagebyadminofpage(createnewspageforAdminOfPage model);
        public Task<Newspage> DeletePageById(int id);
        public Task<Newspage> UpdateNewsPage(int id, UpdateNewsDto model);
        public Task<Newspage> approveRequestfromAdmin(int id,bool State);
        public Task<Newspage> AddPagebyadminofpageWithID(createNewspagewithadminIdDTO model, string Id);
        public Task<Newspage> AddpublisherToNewsPage(int id, string publisherId);
    }
}
