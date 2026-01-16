using News_App.Dto.post;
using News_App.Models;

namespace News_App.Interfaces
{
    public interface IPost
    {
        public Task<List<PostDto>> GetPosts();
        public Task<PostDto2> GetPostWithId(int id);
        public Task<List<PostDto>> GetPostsOfPagebyId(int id);
        public Task<List<PostDto>> GetPostsOfPagebyname(string name);
        public Task<List<PostDto>> GetPostsOfCategorybyId(int id);
        public Task<List<PostDto>> GetPostsOfCategorybyname(string name);
        public Task<Post> createPost(CreatePostDto post);
        public Task<Post> DeletePost(int id);
        public Task<Post> UpdatePost(int id,UpdatePostDto model);
        public Task<List<PostDto>> GetPostsOfPublisher(string IdOfPublisher);

         
    }
}
