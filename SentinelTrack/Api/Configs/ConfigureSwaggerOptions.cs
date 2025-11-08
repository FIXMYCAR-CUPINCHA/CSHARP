using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SentinelTrack.Api.Configs;

public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _provider;

    public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
    {
        _provider = provider;
    }

    public void Configure(SwaggerGenOptions options)
    {
        foreach (var description in _provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(description.GroupName, new OpenApiInfo
            {
                Title = "API do projeto SentinelTrack",
                Version = description.ApiVersion.ToString(),
                Description = "API do projeto SentinelTrack do Challenge da Mottu.",
                Contact = new OpenApiContact
                {
                    Name = "Thomaz Bartol",
                    Email = "rm555323@fiap.com.br"
                }
            });
        }
    }
}