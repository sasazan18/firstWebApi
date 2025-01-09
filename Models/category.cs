using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Ecommerce_Web_API.models
{

// category entity/model
public class Category{
    public Guid Id { get; set; } // guid is used to generate unique id
    public string? Name { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedDate { get; set; }
}

}


// entity: product, category, order, user
// model/entity communicates with the database

