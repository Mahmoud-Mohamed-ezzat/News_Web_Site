using News_App.Dto.category;
using News_App.Dto.Category;
using News_App.Models;

namespace News_App.Mapper.category

{
    public static class CategoryDtoMapper
    {
        public static CategoryDto ToCategoryDto(this Category category)
        {
            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name
            };
        }  public static CategoryDtoById ToCategoryDtoByid(this Category category)
        {
            return new CategoryDtoById
            {
                Name = category.Name
            };
        }
        public static Category ToAddCategoryDto(this AddCategoryDto category)
        {
            return new Category
            {
                Name = category.name,
            };
        }
            public static AddCategoryDto ToUpdateCategoryDto(this Category category)
        {
            return new AddCategoryDto
            {
                name = category.Name,
            };
        }
        
    }
}
