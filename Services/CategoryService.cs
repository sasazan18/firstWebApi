using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce_Web_API.models;
using Ecommerce_Web_API.DTOs;


namespace Ecommerce_Web_API.Services
{
    public class CategoryService
    {
        private static readonly List<Category> _categories = new List<Category>();

        public List<CategoryReadDto> GetAllCategories()
        {
            return _categories.Select(c => new CategoryReadDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
            }).ToList();
        }


        public CategoryReadDto? GetCategoryById(Guid categoryID)
        {
            var foundCategory = _categories.FirstOrDefault(c => c.Id == categoryID);
            if (foundCategory == null)
            {
                return null;
            }

            return new CategoryReadDto
            {
                Id = foundCategory.Id,
                Name = foundCategory.Name,
                Description = foundCategory.Description
            };
        }



        public CategoryReadDto CreateCategory(CategoryCreateDto categoryData)
        {
            var newCategory = new Category
            {
                Id = Guid.NewGuid(),
                Name = categoryData.Name,
                Description = categoryData.Description,
                CreatedDate = DateTime.Now
            };
            _categories.Add(newCategory);

            return new CategoryReadDto
            {
                Id = newCategory.Id,
                Name = newCategory.Name,
                Description = newCategory.Description
            };
        }

        public CategoryReadDto? UpdateCategoryById(Guid categoryID, CategoryUpdateDto categoryData)
        {
            var foundCategory = _categories.FirstOrDefault(c => c.Id == categoryID);
            if (foundCategory == null)
            {
                return null;
            }

            return new CategoryReadDto
            {
                Id = foundCategory.Id,
                Name = categoryData.Name,
                Description = categoryData.Description
            };

        }

        public bool CategoryDeleteByName(string categoryName)
        {
            var foundCategory = _categories.FirstOrDefault(c => c.Name == categoryName);
            if (foundCategory == null)
            {
                return false;
            }
            _categories.Remove(foundCategory);
            return true;

        }




    }
}