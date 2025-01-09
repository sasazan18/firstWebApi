using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce_Web_API.DTOs
{
    public class CategoryUpdateDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
}