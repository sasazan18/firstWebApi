using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce_Web_API.models;
using Ecommerce_Web_API.DTOs;
using Ecommerce_Web_API.Interfaces;
using Ecommerce_Web_API.data;
using Ecommerce_Web_API.Controllers;
using AutoMapper;
using Microsoft.EntityFrameworkCore;


namespace Ecommerce_Web_API.Services
{
    public class CategoryService : InterfaceCategoryService
    {
        // private static readonly List<Category> _categories = new List<Category>();

        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;

        public CategoryService(AppDbContext appDbContext, IMapper mapper)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
        }


        // _categories is following Category model

        public async Task<PagenatedResult<CategoryReadDto>> GetAllCategories (int pageNumber, int pageSize)
        {
            IQueryable<Category> query = _appDbContext.Categories;
            // get total count of categories
            int totalCategories = await query.CountAsync();
            // get total pages
            int totalPages = (int) Math.Ceiling((double) totalCategories/pageSize);

            // pagenation, pageNumber = 1, pageSize = 5
            // let, we have 20 categories
            // skip (pageNumber - 1) * pageSize.Take(pageSize)
            // skip (1-1) * 5 = 0, Take(5) => 0, 1, 2, 3, 4

            var items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            var results = _mapper.Map<List<CategoryReadDto>>(items);
            return new PagenatedResult<CategoryReadDto>
            {
                Items = results,
                TotalCount = totalCategories,
                PageNumber = pageNumber,
                PageSize = pageSize

            };
        }


        public async Task<CategoryReadDto?> GetCategoryById(Guid categoryID)
        {
            var foundCategory = await _appDbContext.Categories.FindAsync(categoryID);
            if (foundCategory == null)
            {
                return null;
            }

            return _mapper.Map<CategoryReadDto>(foundCategory);
        }

        // Here we are converting a DTO into Category model

        public async Task<CategoryReadDto> CreateCategory(CategoryCreateDto categoryData)
        {

            // CategoryCreateDto to Category model
            var newCategory = _mapper.Map<Category>(categoryData);
            
            newCategory.Id = Guid.NewGuid();
            newCategory.CreatedDate = DateTime.UtcNow;

            await _appDbContext.Categories.AddAsync(newCategory);
            await _appDbContext.SaveChangesAsync();
            
            return _mapper.Map<CategoryReadDto>(newCategory);
        }

        public async Task<CategoryReadDto?> UpdateCategoryById(Guid categoryID, CategoryUpdateDto categoryData)
        {
            var foundCategory =await _appDbContext.Categories.FirstOrDefaultAsync(c => c.Id == categoryID);
            if (foundCategory == null)
            {
                return null;
            }

            // foundCategory.Name = categoryData.Name;
            // foundCategory.Description = categoryData.Description;

            _mapper.Map(categoryData, foundCategory);
            _appDbContext.Categories.Update(foundCategory);
            await _appDbContext.SaveChangesAsync();

            // return new CategoryReadDto
            // {
            //     Id = foundCategory.Id,
            //     Name = foundCategory.Name,
            //     Description = foundCategory.Description
            // };

            return _mapper.Map<CategoryReadDto>(foundCategory);
        

        }

        public async Task<bool> CategoryDeleteByName(string categoryName)
        {
            var foundCategory = await _appDbContext.Categories.FirstOrDefaultAsync(c => c.Name == categoryName);
            if (foundCategory == null)
            {
                return false;
            }
            _appDbContext.Categories.Remove(foundCategory);
            await _appDbContext.SaveChangesAsync();
            return true;

        }




    }
}