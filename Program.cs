using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var app = builder.Build();

app.UseDefaultFiles();

app.UseStaticFiles(); // Enables serving from wwwroot

app.UseRouting();

app.MapControllers();

app.Run();