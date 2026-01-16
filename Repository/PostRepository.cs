using Microsoft.EntityFrameworkCore;
using News_App.Dto.post;
using News_App.Interfaces;
using News_App.Mapper.post;
using News_App.Models;
using News_App.services;

namespace News_App.Repository
{
    public class PostRepository : IPost
    {
        readonly NewsContext _context;
        readonly ImagesServices _imagesServices;
        public PostRepository(NewsContext context, ImagesServices imagesServices)
        {
            _context = context;
            _imagesServices = imagesServices;
        }


        public async Task<Post> createPost(CreatePostDto post)
        {
            if (post == null) return null;
            var postEntity = post.ToCreatePostDto();
            var newpost = await _context.Posts.AddAsync(postEntity);
            await _context.SaveChangesAsync();
            return newpost.Entity;
        }

        public async Task<Post> DeletePost(int id)
        {
            var Post = await _context.Posts.FirstOrDefaultAsync(post => post.Id == id);
            if (Post == null) return null;
            _context.Posts.Remove(Post);
            await _imagesServices.DeleteImages(Post.Image);
            await _context.SaveChangesAsync();
            return Post;

        }

        public async Task<List<PostDto>> GetPosts()
        {
            var posts = await _context.Posts.AsNoTracking().Include(p => p.Category).Include(p => p.Publisher).Include(p => p.Newspage).ToListAsync();
            if (posts == null) return null;
            var postsDto = posts.Select(p => p.ToPostDto()).ToList();
            return postsDto;
        }

        public async Task<List<PostDto>> GetPostsOfCategorybyId(int id)
        {
            var posts = await _context.Posts.AsNoTracking().Include(p => p.Category).Include(p => p.Publisher).Include(p => p.Newspage).Where(p => p.CategoryId == id).ToListAsync();
            if (posts == null) return null;
            var PostsDto = posts.Select(p => p.ToPostDto()).ToList();
            return PostsDto;
        }

        public async Task<List<PostDto>> GetPostsOfCategorybyname(string name)
        {
            var posts = await _context.Posts.AsNoTracking().Include(p => p.Category).Include(p => p.Publisher).Include(p => p.Newspage).Where(p => p.Category.Name.ToLower().Contains(name.ToLower())).ToListAsync();
            if (posts == null) return null;
            var PostsDto = posts.Select(p => p.ToPostDto()).ToList();
            return PostsDto;
        }

        public async Task<List<PostDto>> GetPostsOfPagebyId(int id)
        {
            var posts = await _context.Posts.AsNoTracking().Include(p => p.Category).Include(p => p.Publisher).Include(p => p.Newspage).Where(p => p.NewspageId == id).ToListAsync();
            if (posts == null) return null;
            var PostsDto = posts.Select(p => p.ToPostDto()).ToList();
            return PostsDto;
        }
        public async Task<List<PostDto>> GetPostsOfPagebyname(string name)
        {
            var posts = await _context.Posts.AsNoTracking().Include(p => p.Category).Include(p => p.Publisher).Include(p => p.Newspage).Where(p => p.Newspage.Name.ToLower().Contains(name.ToLower())).ToListAsync();
            if (posts == null) return null;
            var PostsDto = posts.Select(p => p.ToPostDto()).ToList();
            return PostsDto;
        }

        public async Task<List<PostDto>> GetPostsOfPublisher(string IdOfPublisher)
        {
            var posts = await _context.Posts.AsNoTracking().Include(p => p.Category).Include(p => p.Publisher).Include(p => p.Newspage).Where(p => p.PublisherId == IdOfPublisher).ToListAsync();
            if (posts == null) return null;
            var PostsDto = posts.Select(p => p.ToPostDto()).ToList();
            return PostsDto;
        }

        public async Task<PostDto2> GetPostWithId(int id)
        {
            var posts = await _context.Posts.AsNoTracking().Include(p => p.Category).Include(p => p.Publisher).Include(p => p.Newspage).FirstOrDefaultAsync(p => p.Id == id);
            if (posts == null) return null;
            return posts.ToPostDto2();
        }

        public async Task<Post> UpdatePost(int id, UpdatePostDto model)
        {
            var post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == id);
            if (post == null) return null;
            await _imagesServices.DeleteImages(post.Image);
            post.Title = model.Title;
            post.Post1 = model.Post1;
            post.Image = await _imagesServices.CreateImages(model.Images);
            await _context.SaveChangesAsync();
            return (post);
        }
    }
}
