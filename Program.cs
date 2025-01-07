var builder = WebApplication.CreateBuilder(args); // create a new web application

builder.Services.AddEndpointsApiExplorer(); // add endpoint api explorer
builder.Services.AddSwaggerGen(); // add swagger gen

var app = builder.Build(); // build the application

if (app.Environment.IsDevelopment()) // check if the environment is development
{
    app.UseSwagger(); // use swagger
    app.UseSwaggerUI(); // use swagger ui
}


app.UseHttpsRedirection(); // listen http and redirect to https


List<Category> categories = new List<Category> // create a list of products
{
    new Category { Id = Guid.NewGuid(), Name = "Electronics", Description = "Electronic Items", CreatedDate = DateTime.Now },
    new Category { Id = Guid.NewGuid(), Name = "Clothes", Description = "Clothes Items", CreatedDate = DateTime.Now },
    new Category { Id = Guid.NewGuid(), Name = "Grocery", Description = "Grocery Items", CreatedDate = DateTime.Now },
    new Category { Id = Guid.NewGuid(), Name = "Furniture", Description = "Furniture Items", CreatedDate = DateTime.Now }
};


// home page
app.MapGet("/", () => {
    return "Home directory: App is working fine!";
}); // map the root path to a get request


// get method
// Read a category by ID -- using GET method => /api/categories/{id}
app.MapGet("/api/categories", () => {
    return Results.Ok(categories); 
   // if we want to return a file we can use Results.File("path to file", "content type");
}); // get method

// create a catagory using post method
app.MapPost("/api/categories", () => {
    var newCategory = new Category { 
        Id = Guid.NewGuid(), Name = "New Category", Description = "New Category Description", CreatedDate = DateTime.Now
    };
    categories.Add(newCategory);
    return Results.Created($"/api/categories/{newCategory.Id}",newCategory);
   // if we want to return a file we can use Results.File("path to file", "content type");
}); // get method


// delete a catagory using post method
app.MapDelete("/api/categories", () => {
    var foundCategory = categories.FirstOrDefault(c => c.Name == "Electronics");
    if(foundCategory == null){
        return Results.NotFound("Category not found");
    }
    else{
        categories.Remove(foundCategory);
        return Results.Ok("Category removed successfully");
    }
   // if we want to return a file we can use Results.File("path to file", "content type");
}); // get method



// Update a catagory using post method
app.MapPut("/api/categories", () => {
    var foundCategory = categories.FirstOrDefault(c => c.Name == "Electronics");
    if(foundCategory == null){
        return Results.NotFound("Category not found");
    }
    else{
        foundCategory.Name = "Updated Electronics";
        foundCategory.Description = "Updated Electronics Description";
        return Results.NoContent();
    }
   // if we want to return a file we can use Results.File("path to file", "content type");
}); // get method


app.Run(); // run the application

// this is a DTO (Data Transfer Object) 
public record Category{
    public Guid Id { get; set; } // guid is used to generate unique id
    public string? Name { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedDate { get; set; }
}

// want to do CRUD operations

// Create a category -- using POST method => /api/categories
// Read a category by ID -- using GET method => /api/categories/{id}
// Update a category by ID -- using PUT method => /api/categories/{id}
// Delete a category by ID -- using DELETE method => /api/categories/{id}
