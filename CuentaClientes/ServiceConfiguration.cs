using Common.Interfaces;
using DBContext;
using Microsoft.EntityFrameworkCore;
using Models;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json;

namespace CuentaClientes.Configurations
{
    public static class ServiceConfiguration
    {
        public static void ConfigureServices(WebApplicationBuilder builder)
        {
            var jwtSettings = builder.Configuration.GetSection("JwtSettings");

            // 1. Configurar Entity Framework con MySQL
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

            // 2. Registrar los servicios
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IAccountService, AccountService>();

            // 3. Configurar controllers con JSON options
            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                    options.JsonSerializerOptions.WriteIndented = true;
                });

            // 4. Configurar Swagger/OpenAPI con documentación XML
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "CuentaClientes API",
                    Version = "v1",
                    Description = "API para gestión de clientes",
                });

                // ✅ ESTA LÍNEA HABILITA LOS ATRIBUTOS
                options.EnableAnnotations();

                // ✅ Incluir autenticación por JWT en Swagger
                options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Description = "Introduce el token JWT con el prefijo Bearer. Ejemplo: \"Bearer {tu token}\""
                });

                options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
                {
                    {
                        new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                        {
                            Reference = new Microsoft.OpenApi.Models.OpenApiReference
                            {
                                Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });

            // 5. Configurar CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            // 6. ✅ Configurar Autenticación JWT
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
     .AddJwtBearer(options =>
     {
         options.TokenValidationParameters = new TokenValidationParameters
         {
             ValidateIssuer = true,
             ValidateAudience = true,
             ValidateLifetime = true,
             ValidateIssuerSigningKey = true,
             ValidIssuer = jwtSettings["Issuer"],
             ValidAudience = jwtSettings["Audience"],
             IssuerSigningKey = new SymmetricSecurityKey(
                 Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]))
         };

         // ✅ Interceptar el challenge para devolver JSON personalizado
         options.Events = new JwtBearerEvents
         {
             OnChallenge = context =>
             {
                 context.HandleResponse(); // Evita la respuesta por defecto 401

                 context.Response.StatusCode = 401;
                 context.Response.ContentType = "application/json";

                 var errorResponse = new
                 {
                     message = "Token inválido o no proporcionado",
                     timestamp = DateTime.UtcNow,
                     type = "AuthError",
                     code = 401,
                 };

                 var json = JsonSerializer.Serialize(errorResponse);
                 return context.Response.WriteAsync(json);
             }
         };
     });
        }
    }
}
