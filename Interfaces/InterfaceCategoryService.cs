using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce_Web_API.Interfaces;
using Ecommerce_Web_API.DTOs;
using Ecommerce_Web_API.Controllers;


namespace Ecommerce_Web_API.Interfaces
{
    public interface InterfaceCategoryService
    {
        Task<PagenatedResult<CategoryReadDto>> GetAllCategories(int pageNumber, int pageSize);
        Task<CategoryReadDto?> GetCategoryById(Guid categoryID);
        Task<CategoryReadDto> CreateCategory(CategoryCreateDto categoryData);
        Task<CategoryReadDto?> UpdateCategoryById(Guid categoryID, CategoryUpdateDto categoryData);
        Task<bool> CategoryDeleteByName(string categoryName);

    }
}