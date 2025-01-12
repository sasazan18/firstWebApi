using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Ecommerce_Web_API.models;
using Ecommerce_Web_API.DTOs;
using Microsoft.VisualBasic;
using Ecommerce_Web_API.Services;
using Ecommerce_Web_API.Interfaces;

//version 1.0

namespace Ecommerce_Web_API.Controllers
{
    [ApiController]
    [Route("v1/api/categories/")]
    public class CategoryController : ControllerBase
    {
        // Creating a private instance of the CategoryService
        private InterfaceCategoryService _categoryService;

        // Constructor to initialize the CategoryService
        public CategoryController(InterfaceCategoryService categoryService){
            _categoryService = categoryService;
        }


        // Get: /api/categories?pageNumber=2&&pageSize=5 => Read Categories
        [HttpGet]
        public async Task <IActionResult> GetCategories([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 5)
        {
            
            var categoryList = await _categoryService.GetAllCategories(pageNumber, pageSize);

            return Ok(ApiResponse<PagenatedResult<CategoryReadDto>>.SuccessResponse(categoryList, 200, "Categories retrieved successfully"));
        }

        // Get: /api/categories/{categoryID} => Read a Category by Id
        [HttpGet("{categoryID:guid}")]
        public async Task<IActionResult> GetCategoryById(Guid categoryID)
        {
            var retrievedCategory = await _categoryService.GetCategoryById(categoryID);
            if (retrievedCategory == null)
            {
                return NotFound(ApiResponse<object>.ErrorResponse(new List<string> {"Category not found!"}, 404, "Validation failed"));
            }
            return Ok(ApiResponse<CategoryReadDto>.SuccessResponse(retrievedCategory, 200, "Category retrieved successfully"));
        }


        // POST: v1/api/categories => Create a category
        [HttpPost]
        public async Task <IActionResult> CreateCategories([FromBody] CategoryCreateDto categoryData)
        {
            var newCategory = await _categoryService.CreateCategory(categoryData);

            return Created($"/api/categories/{newCategory.Id}", ApiResponse<CategoryReadDto>.SuccessResponse(newCategory, 201, "Category created successfully"));
            
        }

        // PUT: /api/categories/{categoryID} => update a category by ID
        [HttpPut("{categoryID}")]
        public async Task <IActionResult> UpdateCategories (Guid categoryID, [FromBody] CategoryUpdateDto categoryData)
        {
            var updatedCategory = await _categoryService.UpdateCategoryById(categoryID, categoryData);
            if (updatedCategory == null)
            {
                return NotFound(ApiResponse<object>.ErrorResponse(new List<string> {"Category not found!"}, 404, "Validation failed"));
            }

            else 
                return Ok(ApiResponse<CategoryReadDto>.SuccessResponse(updatedCategory, 200, "Category updated successfully"));
            
        }

        // DELETE: /api/categories/{categoryName} => DELETE a category by Name
        [HttpDelete("{categoryName}")]
        public async Task<IActionResult> DeleteCategories(string categoryName)
        {
            var isDeleted = await _categoryService.CategoryDeleteByName(categoryName);
            if (isDeleted== false)
                return NotFound(ApiResponse<object>.ErrorResponse(new List<string> {"Category not found!"}, 404, "Validation failed"));
            else
                return Ok(ApiResponse<object>.SuccessResponse(null, 204, "Category deleted successfully"));

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
// request -> controller -> service -> database -> service -> controller -> response