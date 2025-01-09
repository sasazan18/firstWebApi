using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace Ecommerce_Web_API.DTOs
{
    public class CategoryReadDto
    {
    public Guid Id { get; set; }
    
    [Required (ErrorMessage = "Category name is required")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Category name must be between 3 and 50 characters")]
    public required string Name { get; set; }
    [Required (ErrorMessage = "Category description is required")]
    [StringLength(200, MinimumLength = 5, ErrorMessage = "Category description must be between 10 and 200 characters")]
    public required string Description { get; set; }
    }
}