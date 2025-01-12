using Ecommerce_Web_API.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options; // add mvc namespace
using Ecommerce_Web_API.Services;
using Ecommerce_Web_API.DTOs;
using Ecommerce_Web_API.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Ecommerce_Web_API.data;
// add npgsql namespace




var builder = WebApplication.CreateBuilder(args); // create a new web application

builder.Services.AddAutoMapper(typeof(Program)); // add automapper
// add controllers to the services

builder.Services.AddScoped<CategoryService>(); // add scoped services
builder.Services.AddScoped<InterfaceCategoryService, CategoryService>(); 

builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")) ); // add db context




// add services to the controller
builder.Services.AddControllers();

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState.Where(e => e.Value!= null &&  e.Value.Errors.Any())
            .SelectMany(e => e.Value?.Errors!= null ? e.Value.Errors.Select(er => er.ErrorMessage) : new List<string>()).ToList();

        return new BadRequestObjectResult(ApiResponse<object>.ErrorResponse(errors, 400, "Invalid model state"));
    };
});

builder.Services.AddEndpointsApiExplorer(); // add endpoint api explorer
builder.Services.AddSwaggerGen(); // add swagger gen

var app = builder.Build(); // build the application  

if (app.Environment.IsDevelopment()) // check if the environment is development
{
    app.UseSwagger(); // use swagger
    app.UseSwaggerUI(); // use swagger ui
}


app.UseHttpsRedirection(); // listen http and redirect to https


// home page
app.MapGet("/", () => {
    return "Home directory: App is working fine!";
}); // map the root path to a get request


app.MapControllers(); // map the controllers

app.Run(); // run the application

