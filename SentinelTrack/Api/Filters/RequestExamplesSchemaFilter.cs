using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using SentinelTrack.Application.DTOs.Request;

namespace SentinelTrack.Api.Filters;

public sealed class RequestExamplesSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type == typeof(YardRequest))
        {
            schema.Example = Example(new
            {
                name = "Pátio Central",
                address = "Av. Paulista, 1000",
                phoneNumber = "+55 11 4002-8922",
                capacity = 150
            });
        }
        else if (context.Type == typeof(MotoRequest))
        {
            schema.Example = Example(new
            {
                plate = "ABC1D23",
                model = "Honda CG 160",
                color = "Preta",
                yardId = "00000000-0000-0000-0000-000000000001"
            });
        }
        else if (context.Type == typeof(UserRequest))
        {
            schema.Example = Example(new
            {
                name = "Ana Souza",
                email = "ana@sentineltrack.com",
                role = "admin"
            });
        }
    }

    private static Microsoft.OpenApi.Any.OpenApiObject Example(object anon)
    {
        var o = new Microsoft.OpenApi.Any.OpenApiObject();
        foreach (var p in anon.GetType().GetProperties())
        {
            var v = p.GetValue(anon);
            o[p.Name] = v switch
            {
                null => new Microsoft.OpenApi.Any.OpenApiNull(),
                string s => new Microsoft.OpenApi.Any.OpenApiString(s),
                int i => new Microsoft.OpenApi.Any.OpenApiInteger(i),
                _ => new Microsoft.OpenApi.Any.OpenApiString(v.ToString()!)
            };
        }
        return o;
    }
}