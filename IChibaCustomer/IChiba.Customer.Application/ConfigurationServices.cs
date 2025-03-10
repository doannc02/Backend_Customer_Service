using IChiba.Customer.Application.Customers.Queries;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace IChiba.Customer.Application;

public static class ConfigurationServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection service)
    {
        service.AddLogging();
        service.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(GetDetailCustomerQuery).Assembly));

        return service;
    }
}

