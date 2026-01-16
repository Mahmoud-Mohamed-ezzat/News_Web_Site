using Microsoft.AspNetCore.Mvc;
using News_App.Dto.category;
using News_App.Dto.Category;
using News_App.Models;

namespace News_App.Interfaces
{
    public interface ICategory
    {
        public Task<List<CategoryDto>> GetCategories();
        public Task<CategoryDtoById> GetCategoryBYid(int id);
        public Task<Category> DeleteCategory(int id);
        public  Task<Category> AddCategory(AddCategoryDto category);
        public Task<AddCategoryDto> UpdateCategory(string name,int id);
    }
}
