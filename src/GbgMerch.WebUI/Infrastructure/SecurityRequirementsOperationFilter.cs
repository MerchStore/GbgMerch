using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using GbgMerch.WebUI.Authentication.ApiKey;

namespace GbgMerch.WebUI.Infrastructure;

public class SecurityRequirementsOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (context.ApiDescription.ActionDescriptor.GetType().Name.Contains("ControllerActionDescriptor"))
        {
            var methodInfo = context.MethodInfo;
            var controllerType = methodInfo?.DeclaringType;

            if (methodInfo != null)
            {
                var hasAuthorizeAttribute = methodInfo.GetCustomAttribute<AuthorizeAttribute>() != null
                    || controllerType?.GetCustomAttribute<AuthorizeAttribute>() != null;

                if (hasAuthorizeAttribute)
                {
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
    }
}
