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
            
            // if (!string.IsNullOrEmpty(searchValue))
            // {
            //     var searchCategories = categories.Where(c => !string.IsNullOrEmpty(c.Name) && c.Name.Contains(searchValue, StringComparison.OrdinalIgnoreCase)).ToList();
            //     return Ok(searchCategories);
            // }

            var categoryList = categories.Select(c => new CategoryReadDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
            }).ToList();

            return Ok(categoryList);
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
            return Created($"/api/categories/{newCategory.Id}", categoryReadDto);
        }

        // DELETE: /api/categories/{categoryName} => DELETE a category by Name
        [HttpDelete("{categoryName}")]
        public IActionResult DeleteCategories(string categoryName)
        {
            var foundCategory = categories.FirstOrDefault(c => c.Name == categoryName);
            if (foundCategory == null)
            {
                return NotFound("Category not found");
            }
            else
            {
                categories.Remove(foundCategory);
                return Ok("Category removed successfully");
            }
        }

        // PUT: /api/categories/{categoryID} => update a category by ID
        [HttpPut("{categoryID}")]
        public IActionResult UpdateCategories(Guid categoryID, [FromBody] CategoryUpdateDto categoryData)
        {
            if (string.IsNullOrEmpty(categoryData.Name) || string.IsNullOrEmpty(categoryData.Description))
            {
                return BadRequest("Name and Description are required");
            }
            var foundCategory = categories.FirstOrDefault(c => c.Id == categoryID);
            if (foundCategory == null)
            {
                return NotFound("Category not found");
            }
            else
            {

                if (categoryData.Name.Length >= 4)
                {
                    foundCategory.Name = categoryData.Name;
                }
                else
                {
                    return BadRequest("Name should be atleast 4 characters long");
                }

                if (categoryData.Description.Length >= 10)
                {
                    foundCategory.Description = categoryData.Description;
                }
                else
                {
                    return BadRequest("Description should be atleast 10 characters long");
                }
                return NoContent();
            }
        }
    }
}