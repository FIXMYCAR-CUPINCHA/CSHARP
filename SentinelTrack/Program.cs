using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using SentinelTrack.Api.Filters;
using SentinelTrack.Infrastructure.Context;
using SentinelTrack.Application.Interfaces;
using SentinelTrack.Application.Services;
using SentinelTrack.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("SentinelTrackDb"));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<IDataSeeder, DataSeeder>();
builder.Services.AddScoped<IYardService, YardService>();
builder.Services.AddScoped<IMotoService, MotoService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddAutoMapper(typeof(Program));

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
});

builder.Services.AddHealthChecks();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapHealthChecks("/health");
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<IDataSeeder>();
    seeder.Seed();
}

app.Run();