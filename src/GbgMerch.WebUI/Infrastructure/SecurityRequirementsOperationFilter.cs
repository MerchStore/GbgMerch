using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using GbgMerch.WebUI.Authentication.ApiKey;

namespace GbgMerch.WebUI.Infrastructure;

/// <summary>
/// Lägger till API-nyckelkrav i Swagger om [Authorize] används.
/// </summary>
public class SecurityRequirementsOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        // Kontrollera om [Authorize] finns på metod eller controller
        var methodInfo = context.MethodInfo;
        var controllerType = methodInfo?.DeclaringType;

        var hasAuthorize = methodInfo?.GetCustomAttribute<AuthorizeAttribute>() != null
                           || controllerType?.GetCustomAttribute<AuthorizeAttribute>() != null;

        if (hasAuthorize)
        {
            // Lägg till krav på API-nyckel i Swagger
            operation.Security = new List<OpenApiSecurityRequirement>
            {
                new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = ApiKeyAuthenticationDefaults.AuthenticationScheme
                            }
                        },
                        Array.Empty<string>()
                    }
                }
            };
        }
    }
}
