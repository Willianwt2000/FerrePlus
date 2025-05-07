using FerrePlus.Models;
using FerrePlus.Data;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;




var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<FerrePlusDbContext>(options =>
    options.UseSqlServer("Server=localhost; Database=FerrePlus;User Id=Sa;Password=Sa123456;TrustServerCertificate=True"));

builder.Services.AddControllers().AddJsonOptions(options =>
{
  options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
});

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers(); 


using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<FerrePlusDbContext>();
    DbInitializer.Seed(context);
}


app.Run();


//Agregar validacion con jwt ,middleware y filtro