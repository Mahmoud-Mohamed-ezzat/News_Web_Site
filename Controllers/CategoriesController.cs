using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using News_App.Dto.Category;
using News_App.Interfaces;
using News_App.Mapper;
using News_App.Models;
using News_App.Repository;
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;

namespace News_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly NewsContext context;
        private readonly ICategory _repo;
        public CategoriesController(NewsContext context ,ICategory repo) { 
        this.context=context;
            _repo=repo;
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Getcategories() { 
         var categories = await _repo.GetCategories();
            if (categories == null) { return null; }
            return Ok(categories);
        }
        [Authorize]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetcategoryByid(int id) {
            var category=await _repo.GetCategoryBYid(id);
            if (category==null) return NotFound("this category  not exist");
            return Ok(category);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> addcategory([FromBody]AddCategoryDto category) {
            if(!ModelState.IsValid) return BadRequest(ModelState);
            var categorymodel = await _repo.AddCategory(category);
            return Ok("successful adding");
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteCategoryByID(int id) {
            if (!ModelState.IsValid) return BadRequest("this id isn't exist");
            var category=await _repo.DeleteCategory(id);
            if (category==null) return BadRequest("invalid category");
            return Ok($"Successful delete {category.Name}");
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateCategory(int id,[FromBody]string name) {
        if (!ModelState.IsValid) {   return BadRequest(ModelState); }
            var category = await _repo.UpdateCategory(name,id);
            if (category == null) return BadRequest("this item isn't exist");
            return Ok(category);
         }
    }
}