using News_App.Dto.post;
using News_App.Models;
using News_App.services;

namespace News_App.Mapper.post
{
    public static class PostMapper
    {
        private static readonly ImagesServices _imagesServices=new ImagesServices();
        public static PostDto ToPostDto(this Post model)
        {
            return new PostDto
            {
                Id = model.Id,
                Title = model.Title,
                Post1 = model.Post1,
                PublisherName = model.Publisher.UserName,
                PublisherId = model.PublisherId,
                NewspageId = model.NewspageId,
                NewsPageName = model.Newspage.Name,
                CategoryId = model.CategoryId,
                Categoryname = model.Category.Name,
                Images= model.Image,
            };
        }
        public static PostDto2 ToPostDto2(this Post model)
        {
            return new PostDto2
            {
                
                Title = model.Title,
                Post1 = model.Post1,
                PublisherName = model.Publisher.UserName,
                PublisherId = model.PublisherId,
                NewspageId = model.NewspageId,
                NewsPageName = model.Newspage.Name,
                CategoryId = model.CategoryId,
                Categoryname = model.Category.Name,
                Images= model.Image,
            };
        }
        public static Post ToCreatePostDto(this CreatePostDto model)
        {
            return new Post
            {
                Title = model.title,
                Post1 = model.Post1,
                PublisherId = model.PublisherId,
                NewspageId = model.NewsPageId,
                CategoryId = model.CategoryId,
                Image = _imagesServices.CreateImages(model.Images).Result,
            };

        }
        public static UpdatePostDto ToUpdatePostDto(this Post model)
        {          
              return new UpdatePostDto
            {
                Title = model.Title,
                Post1 = model.Post1,
            };
        }
    }
}
