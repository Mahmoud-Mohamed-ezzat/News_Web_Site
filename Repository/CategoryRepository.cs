using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using News_App.Dto.category;
using News_App.Dto.Category;
using News_App.Interfaces;
using News_App.Mapper.category;
using News_App.Models;

namespace News_App.Repository
{
    public class CategoryRepository : ICategory
    {
        private readonly NewsContext _context;
        public CategoryRepository( NewsContext context)=> _context = context;

        public async Task<Category> AddCategory(AddCategoryDto category)
        {
            var categorymodel = category.ToAddCategoryDto();
            await _context.Categories.AddAsync(categorymodel);
            await _context.SaveChangesAsync();
            return categorymodel;
        }

        public async Task<Category> DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return null;
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<List<CategoryDto>> GetCategories()
        {
            var categories = await _context.Categories
                   .AsNoTracking()
                   .Select(c => c.ToCategoryDto())
                   .ToListAsync();
            if (categories == null) return null;
            return categories;
        }

        public async Task<CategoryDtoById> GetCategoryBYid(int id)
        {
            var category = await _context.Categories
              .AsNoTracking()
              .FirstOrDefaultAsync(c => c.Id == id);
            if (category == null) return null;
            return category.ToCategoryDtoByid();
        }

        public async Task<AddCategoryDto> UpdateCategory(string name, int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return null ;
            category.Name = name;
            await _context.SaveChangesAsync();
            return category.ToUpdateCategoryDto();
        }
    }
}
