using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using News_App.Dto.Account;
using News_App.Dto.newsPage;
using News_App.Interfaces;
using News_App.Mapper.newspages;
using News_App.Models;

namespace News_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsPageController : ControllerBase
    {
        readonly INewsPage _newsRepo;
        readonly UserManager<User> _userManager;
        readonly NewsContext _newsContext;
           
        public NewsPageController(INewsPage newsRepo, UserManager<User> userManager, NewsContext newsContext)
        {
            _newsRepo = newsRepo;
            _userManager = userManager;
            _newsContext = newsContext;
        }
        [HttpGet("getpageswithids")]
        public async Task<IActionResult> getpageswithids()
        {
            var newspagesDto = await _newsRepo.GetPageswithids();
            if (newspagesDto == null) return NotFound("not found pages");
            return Ok(newspagesDto);
        }
        [HttpGet("getpages")]
        public async Task<IActionResult> getpages()
        {
            var newspagesDto = await _newsRepo.GetPages();
            if (newspagesDto == null) return NotFound("not found pages");
            return Ok(newspagesDto);
        }
        [HttpGet("PagesUnderRequests")]
        public async Task<IActionResult> GetPagesUnderRequest()
        {
            var newspages = await _newsRepo.GetPagesUnderRequest();
            if (newspages == null) return NotFound("not pages  found");
            return Ok(newspages);
        }
        [HttpGet("getpagesbyPublishers")]
        public async Task<IActionResult> getpagesbyPublishers()
        {
            var newspagesDto = await _newsRepo.getpagesbyPublishers();
            if (newspagesDto == null) return NotFound("not found pages");
            return Ok(newspagesDto);
        }
        [HttpGet("GetpageByid/{id:int}")]
        public async Task<IActionResult> GetpageByid(int id)
        {
            var newspage = await _newsRepo.GetpageByid(id);
            if (newspage == null) return NotFound("this page isn't exist");
            return Ok(newspage);
        }
        [HttpGet("GetpagewithPublishersByid/{id:int}")]
        public async Task<IActionResult> GetpagewithPublishersByid(int id)
        {
            var newspage = await _newsRepo.GetpagewithPublishersByid(id);
            if (newspage == null) return NotFound("this page isn't exist");
            return Ok(newspage);
        }
        [HttpGet("GetPagesOfAdmin")]
        public async Task<IActionResult> GetPagesOfAdmin(string AdminId)
        {
            var newsPages = await _newsRepo.GetPagesOfAdmin(AdminId);
            if (newsPages == null) return NotFound("this pages of admin isn't exist");
            return Ok(newsPages);
        }
        [HttpGet("GetpageByName")]
        public async Task<IActionResult> GetpageByName(string name)
        {
            var newspage = await _newsRepo.GetpageByName(name);
            if (newspage == null) return NotFound("not found page");
            return Ok(newspage);
        }
        [HttpPost("MakePublisherAndAddtoPage")]
        public async Task<IActionResult> MakePublisherAndAddtoPage([FromBody] MakePublisherAndAddtoPageDto request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var db= await _newsContext.Database.BeginTransactionAsync();
            try
            {
             var user = new User
             {
                UserName = request.user.UserName,
                Email = request.user.Email,
                PhoneNumber = request.user.PhoneNumber,
             };
             var createUser = await _userManager.CreateAsync(user, request.user.Password);
             if (createUser.Succeeded)
             {
                var role = await _userManager.AddToRoleAsync(user, "Publisher");
                if (role.Succeeded)
                {
                    var newspage = await _newsRepo.AddpublisherToNewsPage(request.pageId, user.Id);
                    await db.CommitAsync();
                    return Ok($"publisher {request.user.UserName} added to news page {newspage.Name}");
                }
                else
                {
                    return StatusCode(500, role.Errors.ToString());
                }
            }
            else
            {
                return StatusCode(500, createUser.Errors.ToString());
            }
        }  
        catch (Exception ex)
        {
            await db.RollbackAsync();
            throw new Exception(ex.Message);
        }
        }

        [HttpPost("createNewsPageandHisAdmin")]
        //for admin of web 
        public async Task<IActionResult> createNewsPageandHisAdmin([FromBody] CreatePageandSignupFoRAdminOfWebDto request )
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
                var db= await _newsContext.Database.BeginTransactionAsync();
            try
            {
                // Create the user (admin)
                var user = new User
                {
                    UserName = request.user.UserName,
                    Email = request.user.Email,
                    PhoneNumber = request.user.PhoneNumber,
                };
                var createUser = await _userManager.CreateAsync(user, request.user.Password);
                if (createUser.Succeeded)
                {
                    var role = await _userManager.AddToRoleAsync(user, "AdminOfPage");
                    if (role.Succeeded)
                    {
                        var page = await _newsRepo.AddPagebyadminofpageWithID(request.page, user.Id);
                       await db.CommitAsync();
                        return Ok($"News page {page.Name} and admin created successfully");
                    }
                    else
                    {
                        return StatusCode(500, role.Errors.ToString());
                    }
                }
                else
                {
                    return StatusCode(500, createUser.Errors.ToString());
                }
            }
            catch (Exception ex)
            {
                await db.RollbackAsync();
                throw new Exception(ex.Message);
            }
        }
        // [HttpPost] // This endpoint was causing errors when mapping controllers in Program.cs, so it has been commented out or removed.
        [HttpPost("AddPage")]
        public async Task<IActionResult> AddPage(CreateNewspageDto model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var newspage = await _newsRepo.AddPage(model);
            return Ok($"newspage {newspage.Name} added successfully");
        }
        [HttpPost("AddPagebyAdminofpage")]
        public async Task<IActionResult> AddPagebyAdminofpage(createnewspageforAdminOfPage model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var newspage = await _newsRepo.AddPagebyadminofpage(model);
            return Ok($"newspage {newspage.Name} under the revision from Admin");
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeletePageById(int id)
        {
            if (!ModelState.IsValid) return BadRequest();
            var newspage = await _newsRepo.DeletePageById(id);
            if (newspage == null) return NotFound("not found page");
            return Ok($"newspage {newspage.Name} deleted successfully");
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateNewsPage(int id, UpdateNewsDto model)
        {
            if (!ModelState.IsValid) return BadRequest();
            var newspage = await _newsRepo.UpdateNewsPage(id, model);
            if (newspage == null) return NotFound("not found page");
            return Ok($"newspage {newspage.Name} updated successfully");
        }
        [HttpPut("approveRequestfromAdmin")]
        public async Task<IActionResult> approveRequestfromAdmin([FromBody] int id, bool State)
        {
            if (!ModelState.IsValid) return BadRequest();
            var newspage = await _newsRepo.approveRequestfromAdmin(id, State);
            if (newspage == null) return NotFound("this request isn't exist");
            return Ok($"{newspage.Name} now is created");
        }
        [HttpPost("Add publisher to news page")]
        public async Task<IActionResult> AddpublisherToNewsPage([FromBody] AddPublisherToPagewithId request)
        {
            if (!ModelState.IsValid) return BadRequest();
            var newspage = await _newsRepo.AddpublisherToNewsPage(request.IdPage,request.IdUser );
            if (newspage == null) return NotFound("this news page isn't exist");
            var publisher = await _userManager.FindByIdAsync(request.IdUser);
            return Ok($"publisher {publisher.UserName} added to news page {newspage.Name}");
        }
    }
}