using Microsoft.EntityFrameworkCore;
using Iltuttofare.Api.Data;
using Microsoft.OpenApi.Models;
using Iltuttofare.Api.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Iltuttofare API", Version = "v1" });

    // Configura JWT no Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Insira o token JWT no campo abaixo:\n\nBearer {seu token}"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
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

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
});

var key = builder.Configuration["Jwt:Key"];
var keyBytes = Encoding.UTF8.GetBytes(key!);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(keyBytes)
        };
    });

builder.Services.AddAuthorization();
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();
    SeedData(context);
}

void SeedData(AppDbContext context)
{
    if (context.Categorias.Any())
        return; // já populado

    var reparos = new Categoria
    {
        Nome = "Reparos e Reformas",
        Subcategorias = new List<Subcategoria>
        {
            new Subcategoria { Nome = "Elétrica" },
            new Subcategoria { Nome = "Hidráulica" },
            new Subcategoria { Nome = "Pintura" },
            new Subcategoria { Nome = "Pedreiro" },
            new Subcategoria { Nome = "Marcenaria" }
        }
    };

    var limpeza = new Categoria
    {
        Nome = "Limpeza",
        Subcategorias = new List<Subcategoria>
        {
            new Subcategoria { Nome = "Limpeza residencial" },
            new Subcategoria { Nome = "Limpeza comercial" },
            new Subcategoria { Nome = "Pós-obra" }
        }
    };

    var tecnologia = new Categoria
    {
        Nome = "Tecnologia",
        Subcategorias = new List<Subcategoria>
        {
            new Subcategoria { Nome = "Suporte técnico" },
            new Subcategoria { Nome = "Instalação de redes" },
            new Subcategoria { Nome = "Configuração de dispositivos" }
        }
    };

    context.Categorias.AddRange(reparos, limpeza, tecnologia);
    context.SaveChanges();
}

// Habilita Swagger em ambiente de desenvolvimento e produção
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Iltuttofare API v1");
});

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();
