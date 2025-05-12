using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using ReviewApiFunction;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Hosting;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults(worker =>
    {
        // Lägg till Swagger/OpenAPI
        worker.Services.AddSingleton<IOpenApiConfigurationOptions, SwaggerConfiguration>();
        worker.Services.AddSingleton<IOpenApiCustomUIOptions, SwaggerUIConfiguration>();
    })
    .Build();

host.Run();


builder.Build().Run();
