using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WishlistProvider.Data.Contexts;
using WishlistProvider.Services;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices((context, services) =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        services.AddDbContext<DataContext>(x =>
        {
            x.UseSqlServer(Environment.GetEnvironmentVariable("SqlServer"));
        });

        services.AddScoped<IAddToWishlistService, AddToWishlistService>();
    })
    .Build();

host.Run();
