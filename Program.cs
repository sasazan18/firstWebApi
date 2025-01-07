var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();


app.UseHttpsRedirection(); // listen http and redirect to https

app.Run();
