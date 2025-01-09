using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options; // add mvc namespace


var builder = WebApplication.CreateBuilder(args); // create a new web application

// add services to the controller
builder.Services.AddControllers();

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState.Where(e => e.Value!= null &&  e.Value.Errors.Any())
            .Select(e => new
            {
                Field = e.Key,
                Error_Messages = e.Value!=null? e.Value.Errors.Select(er => er.ErrorMessage).ToList(): new List<string>()
            }).ToList();

        var error_string = string.Join("\n", errors.Select(e => $"{e.Field}: {string.Join(": ", e.Error_Messages)}"));

        return new BadRequestObjectResult(error_string);
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

