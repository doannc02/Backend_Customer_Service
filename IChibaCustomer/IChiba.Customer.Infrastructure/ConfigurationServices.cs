using IChiba.Customer.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Dapr.Client;
using Dapr.AspNetCore;

namespace IChiba.Customer.Infrastructure;

public static class ConfigurationServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDaprClient();

        // ✅ Đúng cách: Gọi AddDapr() sau AddControllers()
        services.AddControllers()
                .AddDapr();

        //services.Configure<ConsumerPubSubNameSetting>(configuration.GetSection("Dapr:ConsumerPubSubNameSetting"));

        services.AddDbContext<CustomerDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        return services;
    }
}
