using Cine.Data.Models;
using Cine.Data.Repositories.Implementations;
using Cine.Data.Repositories.Interfaces;
using Cine.Data.Services.Implementations;
using Cine.Data.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    // 1. Define el esquema de seguridad (cómo se pasa el token)
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Ingresá el token JWT de esta manera: Bearer {tu_token_aqui}"
    });

    // 2. Aplica el requerimiento de seguridad a nivel global para Swagger
    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
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

// Inyección a la base de datos
builder.Services.AddDbContext<CineDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Inyecciones de dependencia
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IPeliculaRepository, PeliculaRepository>();
builder.Services.AddScoped<IPeliculaService, PeliculaService>();
builder.Services.AddScoped<ICatalogoRepository, CatalogoRepository>();
builder.Services.AddScoped<IButacaRepository, ButacaRepository>();
builder.Services.AddScoped<IButacaService,ButacaService>();
builder.Services.AddScoped<IFuncionRepository,FuncionRepository>();
builder.Services.AddScoped<IFuncionService,FuncionService>();
builder.Services.AddScoped<IReseniaRepository, ReseniaRepository>();
builder.Services.AddScoped<IReseniaService, ReseniaService>();
builder.Services.AddScoped<ICompraRepository, CompraRepository>();
builder.Services.AddScoped<ICompraService, CompraService>();
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IClienteService, ClienteService>();
builder.Services.AddScoped<IProductoShopRepository, ProductoShopRepository>();
builder.Services.AddScoped<IProductoShopService, ProductoShopService>();
builder.Services.AddScoped<IComboRepository, ComboRepository>();
builder.Services.AddScoped<IComboService, ComboService>();
builder.Services.AddScoped<IDashboardRepository, DashboardRepository>();
builder.Services.AddScoped<IDashboardService, DashboardService>();

// Inyección de configuración JWT
// La sección en appsettings.json se llama "Jwt" (la misma que usa UsuarioService para firmar el token).
// Antes se leía "JwtSettings", que no existe: se validaba con otra clave y sin Issuer/Audience,
// por lo que ningún token generado en el login iba a ser aceptado.
var jwtSettings = builder.Configuration.GetSection("Jwt");
var secretKey = jwtSettings["Key"]
    ?? throw new InvalidOperationException("Falta configurar 'Jwt:Key' en appsettings.json.");

builder.Services.AddAuthorization();


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
{
    ValidateIssuer = true,
    ValidateAudience = true,
    ValidateLifetime = true,
    ValidateIssuerSigningKey = true,
    ValidIssuer = jwtSettings["Issuer"],
    ValidAudience = jwtSettings["Audience"],
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("Front", policy => policy
        .WithOrigins("http://127.0.0.1:5500", "http://localhost:5500")
        .AllowAnyHeader()
        .AllowAnyMethod());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.UseHttpsRedirection();

// Sirve los archivos de wwwroot (las portadas de las películas quedan en wwwroot/portadas).
// La carpeta se crea acá por si no existe: si falta al arrancar, ASP.NET no serviría ningún archivo.
var carpetaWwwroot = Path.Combine(app.Environment.ContentRootPath, "wwwroot");
Directory.CreateDirectory(Path.Combine(carpetaWwwroot, "portadas"));
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(carpetaWwwroot)
});

app.UseCors("Front");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
