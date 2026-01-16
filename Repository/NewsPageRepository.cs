using Microsoft.EntityFrameworkCore;
using News_App.Dto.newsPage;
using News_App.Interfaces;
using News_App.Mapper.newspages;
using News_App.Models;
using Microsoft.AspNetCore.Identity;
using News_App.Dto.Account;

namespace News_App.Repository
{
    public class NewsPageRepository : INewsPage
    {
        readonly NewsContext _context;

        public NewsPageRepository(NewsContext context, UserManager<User> userManager)
        {
            _context = context;
        }

        public async Task<List<NewsPageDto3>> GetPagesUnderRequest()
        {
            var newspages = await _context.Newspages.ToListAsync();
            if (newspages == null) { return null; }
            var pages = newspages.Where(n => n.Iscreated == false).Select(n => n.TonewspageDto3()).ToList();
            return pages;
        }
        public async Task<Newspage> GetpagewithPublishersByid(int id)
        {
            var newspage = await _context.Newspages.Include(n => n.Publishers).AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
            if (newspage == null) return null;
            return newspage;
        }
        public async Task<Newspage> AddPage(CreateNewspageDto model)
        {
            var entry = await _context.Newspages.AddAsync(model.ToCreateNewsPages());
            await _context.SaveChangesAsync();
            return entry.Entity;
        }

        public async Task<Newspage> DeletePageById(int id)
        {
            var newspage = await _context.Newspages.FirstOrDefaultAsync(p => p.Id == id);
            if (newspage == null) return null;
            _context.Remove(newspage);
            await _context.SaveChangesAsync();
            return newspage;
        }

        public async Task<newspageDto2?> GetpageByid(int id)
        {
            var newspage = await _context.Newspages.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
            if (newspage == null) return null;
            return newspage.ToNewsPageDto2();
        }

        public async Task<Newspage?> GetpageByName(string name)
        {
            var newspage = await _context.Newspages.AsNoTracking().FirstOrDefaultAsync(p => p.Name.ToLower().Contains(name.ToLower()));
            if (newspage == null) return null;
            return newspage;
        }


        public async Task<List<newspageDto>?> GetPageswithids()
        {
            var newsPages = await _context.Newspages.AsNoTracking().ToListAsync();
            var newspagesDto = newsPages.Select(n => n.ToNewsPageDto()).ToList();
            if (newsPages == null) return null;
            return newspagesDto;
        }
        public async Task<List<newspageDtowithpublisher>?> getpagesbyPublishers()
        {
            var newsPages = await _context.Newspages.Include(n => n.Publishers).AsNoTracking().ToListAsync();
            var newspagesDto = newsPages.Select(n => n.ToNewsPageDtoWithpublisher()).ToList();
            if (newsPages == null) return null;
            return newspagesDto;
        }

        public async Task<List<Newspage>?> GetPagesOfAdmin(string AdminId)
        {
            var newsPages = await _context.Newspages.AsNoTracking().Where(p => p.AdminId == AdminId).ToListAsync();
            if (newsPages == null) return null;
            return newsPages;
        }

        public async Task<Newspage> UpdateNewsPage(int id, UpdateNewsDto model)
        {
            var newspage = await _context.Newspages.FirstOrDefaultAsync(p => p.Id == id);
            if (newspage == null) return null;
            newspage.Name = model.Name;
            newspage.AdminId = model.AdminId;
            newspage.Iscreated = model.Iscreated;
            await _context.SaveChangesAsync();
            return newspage;
        }

        public async Task<List<newspageDto2>> GetPages()
        {
            var newsPages = await _context.Newspages.AsNoTracking().ToListAsync();
            var newspagesDto = newsPages.Where(n => n.Iscreated == true).Select(n => n.ToNewsPageDto2()).ToList();
            if (newsPages == null) return null;
            return newspagesDto;
        }
        public async Task<Newspage> AddPagebyadminofpageWithID(createNewspagewithadminIdDTO model, string Id)
        {
            var entry = await _context.Newspages.AddAsync(model.TocreateNewspagewithadminIdDTO(Id));
            await _context.SaveChangesAsync();
            return entry.Entity;
        }
        public async Task<Newspage> AddPagebyadminofpage(createnewspageforAdminOfPage model)
        {
            var entry = await _context.Newspages.AddAsync(model.ToCreatenewspageforAdminOfPage());
            await _context.SaveChangesAsync();
            return entry.Entity;
        }
        public async Task<Newspage> approveRequestfromAdmin(int id, bool State)
        {
            var page = await _context.Newspages.Include(n => n.Admin).FirstOrDefaultAsync(n => n.Id == id);
            if (page == null) { return null; }
            page.Iscreated = true;
            await _context.SaveChangesAsync();
            return page;
        }
        public async Task<Newspage> AddpublisherToNewsPage(int id, string publisherId)
        {
            var newspage = await _context.Newspages.Include(n => n.Publishers).FirstOrDefaultAsync(n => n.Id == id);
            if (newspage == null) return null;
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == publisherId);
            if (user == null) return null;
            if (!newspage.Publishers.Contains(user))
            {
                newspage.Publishers.Add(user);
            }
            await _context.SaveChangesAsync();
            return newspage;
        }
       
    }
}
