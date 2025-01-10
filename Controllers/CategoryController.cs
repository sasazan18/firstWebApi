using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Ecommerce_Web_API.models;
using Ecommerce_Web_API.DTOs;
using Microsoft.VisualBasic;

namespace Ecommerce_Web_API.Controllers
{
    [ApiController]
    [Route("api/categories/")]
    public class CategoryController : ControllerBase
    {
        // create a list of categories
        private static List<Category> categories = new List<Category>(); // create a list of products
        // Get: /api/categories => Read Categories
        [HttpGet]
        public IActionResult GetCategories([FromQuery] string searchValue = "")
        {
            
            var categoryList = categories.Select(c => new CategoryReadDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
            }).ToList();

            return Ok(ApiResponse<List<CategoryReadDto>>.SuccessResponse(categoryList, 200, "Categories retrieved successfully"));
        }

        // POST: /api/categories => Create a category
        [HttpPost]
        public IActionResult CreateCategories([FromBody] CategoryCreateDto categoryData)
        {
            if(!ModelState.IsValid){
                
                var errors = ModelState.Where(e => e.Value.Errors.Any())
                    .Select(e => new { 
                        Field = e.Key, 
                        Error_Messages = e.Value.Errors.Select(er => er.ErrorMessage).ToList()
                    }).ToList();
                
                var error_string = string.Join("\n", errors.Select(e => $"{e.Field}: {string.Join(": ", e.Error_Messages)}"));
                
                
                return BadRequest(error_string);
            }

            var newCategory = new Category
            {
                Id = Guid.NewGuid(),
                Name = categoryData.Name,
                Description = categoryData.Description,
                CreatedDate = DateTime.Now
            };
            categories.Add(newCategory);

            var categoryReadDto = new CategoryReadDto
            {
                Id = newCategory.Id,
                Name = newCategory.Name,
                Description = newCategory.Description
            };
            return Created($"/api/categories/{newCategory.Id}", ApiResponse<CategoryReadDto>.SuccessResponse(categoryReadDto, 201, "Category created successfully"));
        }

        // DELETE: /api/categories/{categoryName} => DELETE a category by Name
        [HttpDelete("{categoryName}")]
        public IActionResult DeleteCategories(string categoryName)
        {
            var foundCategory = categories.FirstOrDefault(c => c.Name == categoryName);
            if (foundCategory == null)
            {
                return NotFound(ApiResponse<object>.ErrorResponse("Category not found!", 400, "Validation failed"));
            }
            else
            {
                categories.Remove(foundCategory);
                return Ok(ApiResponse<object>.SuccessResponse(null, 204, "Category deleted successfully"));
            }
        }

        // PUT: /api/categories/{categoryID} => update a category by ID
        [HttpPut("{categoryID}")]
        public IActionResult UpdateCategories(Guid categoryID, [FromBody] CategoryUpdateDto categoryData)
        {
            var foundCategory = categories.FirstOrDefault(c => c.Id == categoryID);
            if (foundCategory == null)
            {
                return NotFound(ApiResponse<object>.ErrorResponse("Category not found!", 400, "Validation failed"));
            }
            else
            {
             foundCategory.Name = categoryData.Name;
             foundCategory.Description = categoryData.Description;
             return Ok(ApiResponse<object>.SuccessResponse(null, 204, "Category updated successfully"));
            }
        }
    }
}