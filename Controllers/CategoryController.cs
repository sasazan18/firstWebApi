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
    [Route("v1/api/categories/")]
    public class CategoryController : ControllerBase
    {
        // create a list of categories
       // create a list of products
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

        // Get: /api/categories/{categoryID} => Read a Category by Id
        [HttpGet("{categoryID:guid}")]
        public IActionResult GetCategoryById(Guid categoryID)
        {
            var foundCategory = categories.FirstOrDefault(c => c.Id == categoryID);
            if (foundCategory == null)
            {
                return NotFound(ApiResponse<object>.ErrorResponse(new List<string> {"Category not found!"}, 404, "Validation failed"));
            }
            else
            {
                var categoryReadDto = new CategoryReadDto
                {
                    Id = foundCategory.Id,
                    Name = foundCategory.Name,
                    Description = foundCategory.Description
                };
                return Ok(ApiResponse<CategoryReadDto>.SuccessResponse(categoryReadDto, 200, "Category retrieved successfully"));
            }
        }


        // POST: v1/api/categories => Create a category
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
                return NotFound(ApiResponse<object>.ErrorResponse(new List<string> {"Category not found!"}, 404, "Validation failed"));
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
                return NotFound(ApiResponse<object>.ErrorResponse(new List<string> {"Category not found!"}, 404, "Validation failed"));
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


// URL Name Best Practices
// 1. use descriptive name
// 2. use plurals
// 3. plurals/{singularNoun} => /categories/{categoryID}
// 4. use hiphens for multi-word names to improve readability => /product-categories
// 5. versioning => /v1/categories
// 6. Avoid verbs in URL => /create-category (bad practice) || /categories (POST) (good practice)


// GET /api/categories -> controller -> response 
// controller basically retrieves data from the database using a service and sends it back to the client
// so,service is the one that interacts with the database
// controller -> service -> database -> service -> controller -> response