using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SentinelTrack.Api.Filters;
using SentinelTrack.Application.Interfaces;
using SentinelTrack.Application.Services;
using SentinelTrack.Infrastructure.Context;
using SentinelTrack.Infrastructure.Data;
using SentinelTrack.Infrastructure.Security;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ==========================
// DATABASE
// ==========================
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseInMemoryDatabase("SentinelTrackDb"));

builder.Services.AddScoped<IDataSeeder, DataSeeder>();

// ==========================
// SERVICES
// ==========================
builder.Services.AddScoped<IYardService, YardService>();
builder.Services.AddScoped<IMotoService, MotoService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// ==========================
// CONTROLLERS
// ==========================
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// ==========================
// SWAGGER
// ==========================
builder.Services.AddSwaggerGen(o =>
{
    o.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "API do projeto SentinelTrack",
        Version = "v1",
        Description = "API do projeto SentinelTrack do Challenge da Mottu.",
        Contact = new OpenApiContact
        {
            Name = "Thomaz Bartol",
            Email = "rm555323@fiap.com.br"
        }
    });

    o.SchemaFilter<RequestExamplesSchemaFilter>();
    o.EnableAnnotations();

    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    o.IncludeXmlComments(xmlPath);
    
    o.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Digite 'Bearer' + espaço + seu token JWT.\n\nExemplo: 'Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...'"
    });

    o.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            new string[] {}
        }
    });
});

// ==========================
// HEALTH CHECKS
// ==========================
builder.Services.AddHealthChecks();

// ==========================
// JWT CONFIGURATION
// ==========================
var jwtSection = builder.Configuration.GetSection("Jwt");
builder.Services.Configure<JwtSettings>(jwtSection);

var jwtSettings = jwtSection.Get<JwtSettings>() ?? new JwtSettings();
var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = signingKey
    };
});

// ==========================
// APP BUILD
// ==========================
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapHealthChecks("/health");
app.MapControllers();

// ==========================
// DATA SEEDER
// ==========================
using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<IDataSeeder>();
    seeder.Seed();
}

app.Run();
