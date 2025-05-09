using FerrePlus.Models;
using FerrePlus.Data;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using FerrePlus.Middleware;
using System.Text;
using Microsoft.AspNetCore.Builder;





var builder = WebApplication.CreateBuilder(args);

//Middleware
// builder.Services.AddScoped<IMessageWriter, LoggingMessageWriter>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<FerrePlusDbContext>(options =>
    options.UseSqlServer("Server=localhost; Database=FerrePlus;User Id=Sa;Password=Sa123456;TrustServerCertificate=True"));

builder.Services.AddControllers().AddJsonOptions(options =>
{
  options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
});

builder.Services.AddSwaggerGen(option =>
{
  option.SwaggerDoc("v1", new OpenApiInfo { Title = "Tu API", Version = "v1" });

  // 🛡️ Configura esquema JWT Bearer
  option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
  {
    Name = "Authorization",
    Type = SecuritySchemeType.Http,
    Scheme = "Bearer",
    BearerFormat = "JWT",
    In = ParameterLocation.Header,
    Description = "Ingresa tu token JWT en este formato: Bearer {tu_token}"
  });

  // Aplica la seguridad a todos los endpoints por defecto
  option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
      options.TokenValidationParameters = new TokenValidationParameters
      {
        ValidateIssuer = true,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("TuClaveSecretaSuperSeguraAquiRapidoSeguroYConfiable")),
        ValidIssuer = "mirealkotize"
      };
    });



builder.Services.AddCors(options =>
{
  options.AddPolicy("dejaloentra", policy =>
  {
    policy.WithOrigins("*")
    .AllowAnyHeader()
    .AllowAnyMethod();

  });
}
);


var app = builder.Build();
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
  var context = scope.ServiceProvider.GetRequiredService<FerrePlusDbContext>();
  DbInitializer.Seed(context);
}


app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();


app.UseMiddleware<LogginMiddleware>();


app.Run();


//Agregar validacion con jwt ,middleware y filtro