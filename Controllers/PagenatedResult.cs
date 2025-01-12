using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce_Web_API.Controllers
{
    public class PagenatedResult<T>
    {
        public IEnumerable<T> Items { get; set; } = new List<T>();

        public int TotalCount { get; set; } // total number of items
        public int PageNumber { get; set; }   // current page number
        public int PageSize { get; set; } // how many items per page
        public int TotalPages => (int) Math.Ceiling((double)TotalCount/PageSize) ;
        // total number of pages. Frontend Button will be created based on this number


        
    }
}