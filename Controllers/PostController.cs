using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using News_App.Dto.post;
using News_App.Interfaces;
using News_App.Mapper.post;

namespace News_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        readonly IPost _repo;
        public PostController(IPost repo)
        {
            _repo = repo;
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetPosts()
        {
            var posts = await _repo.GetPosts();
            if (posts == null) { return NotFound(); }
            return Ok(posts);
        }
        [Authorize]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetPostWithId(int id)
        {
            var post = await _repo.GetPostWithId(id);
            if (post == null) { return NotFound(); }
            return Ok(post);
        }

        [Authorize]
        [HttpGet("GetPostsbyIdPages/{id:int}")]
        public async Task<IActionResult> GetPostsOfPagebyId(int id)
        {
            var posts = await _repo.GetPostsOfPagebyId(id);
            if (posts == null) { return NotFound(); }
            return Ok(posts);
        }
        [Authorize]
        [HttpGet("GetPostsbyIdOfPublisher")]
        public async Task<IActionResult> GetPostsbyIdOfPublisher( string id)
        {
            var posts = await _repo.GetPostsOfPublisher(id);
            if (posts == null) { return NotFound(); }
            return Ok(posts);
        }
        [Authorize]
        [HttpGet("GetPostsbyNameofPage")]
        public async Task<IActionResult> GetPostsOfPagebyName(string name)
        {
            var posts = await _repo.GetPostsOfPagebyname(name);
            if (posts == null) { return NotFound("this page isn't exist"); }
            return Ok(posts);
        }
        [Authorize]
        [HttpGet("GetPostsbyIdCategory/{id:int}")]
        public async Task<IActionResult> GetPostsOfCategorybyId(int id)
        {
            var posts = await _repo.GetPostsOfCategorybyId(id);
            if (posts == null) { return NotFound(); }
            return Ok(posts);
        }
        [Authorize]
        [HttpGet("GetPostsbyNameofCategory")]
        public async Task<IActionResult> GetPostsOfCategorybyName(string name)
        {
            var posts = await _repo.GetPostsOfCategorybyname(name);
            if (posts == null) { return NotFound("this page isn't exist"); }
            return Ok(posts);
        }
        [Authorize(Roles = "Publisher")]
        [HttpPost]
        public async Task<IActionResult> createPost(CreatePostDto model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var post = await _repo.createPost(model);
            return Ok(post);
        }
        [Authorize(Roles = "Publisher,AdminOfPage,Admin")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> deletePost(int id)
        {
            if (ModelState.IsValid) return BadRequest(ModelState);
            var post = await _repo.DeletePost(id);
            if (post == null) { return NotFound(); }
            return Ok($"this post with title {post.Title} deleted successfully");
        }
        [Authorize(Roles = "Publisher,AdminOfPage")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdatePost(int id, UpdatePostDto model)
        {
            if (!ModelState.IsValid) return NotFound();
            var post = await _repo.UpdatePost(id, model);
            if (post == null) return BadRequest();
            return Ok($"the post with title : {post.Title} is updated");
        }
    }
}
