using IntelTaskUCR.Domain.Interfaces;
using IntelTaskUCR.Infrastructure.Context;
using IntelTaskUCR.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using DinkToPdf;
using DinkToPdf.Contracts;


var builder = WebApplication.CreateBuilder(args);

// 1) Configura CORS para permitir Angular
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy
            .WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});


// 2) Agrega DbContext y repositorios
builder.Services.AddDbContext<IntelTaskDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DemoConnection")));

builder.Services.AddScoped<IDemoRepository, DemoRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IRolesRepository, RolesRepository>();
builder.Services.AddScoped<IAccionRepository, AccionRepository>();
builder.Services.AddScoped<IEstadoRepository, EstadoRepository>();
builder.Services.AddScoped<INotificacionRepository, NotificacionRepository>();
builder.Services.AddScoped<IOficinaRepository, OficinaRepository>();
builder.Services.AddScoped<IPantallaRepository, PantallaRepository>();
builder.Services.AddScoped<IPermisoRepository, PermisoRepository>();
builder.Services.AddScoped<IPrioridadRepository, PrioridadRepository>();
builder.Services.AddScoped<ITareaRepository, TareaRepository>();
builder.Services.AddScoped<ITareaIncumplimientoRepository, TareaIncumplimientoRepository>();
builder.Services.AddScoped<ITareaJustificacionRechazoRepository, TareaJustificacionRechazoRepository>();
builder.Services.AddScoped<ITareaSeguimientoRepository, TareaSeguimientoRepository>();
builder.Services.AddScoped<ITipoDocumentoRepository, TipoDocumentoRepository>();
builder.Services.AddScoped<ITINotificacionUsuarioRepository, TINotificacionUsuarioRepository>();
builder.Services.AddScoped<ITIRolPantallaAccionRepository, TIRolPantallaAccionRepository>();
builder.Services.AddScoped<ITIUsuarioOficinaRepository, TIUsuarioOficinaRepository>();
builder.Services.AddScoped<IFrecuenciaRecordatorioRepository, FrecuenciaRecordatorioRepository>();
builder.Services.AddScoped<IDiaNoHabilRepository, DiaNoHabilRepository>();
builder.Services.AddScoped<IComplejidadRepository, ComplejidadRepository>();
builder.Services.AddScoped<IAdjuntoRepository, AdjuntoRepository>();
builder.Services.AddScoped<IBitacoraAccionRepository, BitacoraAccionRepository>();
builder.Services.AddScoped<IBitacoraCambioEstadoRepository, BitacoraCambioEstadoRepository>();
builder.Services.AddScoped<IAdjuntoXTareaRepository, AdjuntoXTareaRepository>();
builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));

// 3) JWT Authentication
var jwtKey = builder.Configuration["Jwt:Key"];
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

// 4) MVC & Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "IntelTaskUCR", Version = "v1" });

    //  Configura el esquema de seguridad para JWT
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Ingrese: **Bearer** + espacio + su token JWT"
    });

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


var app = builder.Build();

// 5) Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles(); // para archivos wwwroot

// para carpeta "uploads"
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.ContentRootPath, "uploads")),
    RequestPath = "/uploads"
});

app.UseHttpsRedirection();

app.UseCors("AllowAngular");

app.UseAuthentication(); // ?? obligatorio antes de Authorization
app.UseAuthorization();

app.MapControllers();

app.Run();
