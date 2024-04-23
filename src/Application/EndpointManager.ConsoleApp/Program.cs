using ApplicationManager.Infrastructure.Repository;
using EndpointManager.ConsoleApp;
using EndpointManager.Domain.Repository;
using EndpointManager.Domain.Service;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using IHost host = CreateHostBuilder(args).Build();
using var scope = host.Services.CreateScope(); 

var services = scope.ServiceProvider;

try
{
    services.GetRequiredService<App>().Run(args);
}
catch (Exception ex)
{
    Console.WriteLine(ex);
}

static IHostBuilder CreateHostBuilder(string[] args)
{
    return Host.CreateDefaultBuilder(args)
        .ConfigureServices((_, services) =>
        {
            services.AddSingleton<IModelRepository, ModelRepositoryInMemory>();
            services.AddSingleton<IEndpointRepository, EndpointRepositoryInMemory>();
            services.AddSingleton<IEndpointService, EndpointService>();
            services.AddSingleton<App>();
        }
    );
}