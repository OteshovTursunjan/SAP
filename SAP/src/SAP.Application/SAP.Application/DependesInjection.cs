using Microsoft.AspNetCore.Hosting;

using Microsoft.Extensions.DependencyInjection;
using SAP.Application.Model.User;
using SAP.Application.Service;
using SAP.Application.Service.lmpl;

namespace SAP.Application;

public static class DependesInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IWebHostEnvironment env)
    {
        services.AddServices(env);
        services.RegisterCaching();
        return services;
    }
    private static void AddServices(this IServiceCollection services, IWebHostEnvironment env)
    {
        services.AddScoped<IUserService,UserService>();
        services.AddScoped<IEmployeeService,EmployeeService>();
    }
    private static void RegisterCaching(this IServiceCollection services)
    {
        // Регистрация кэша в памяти
        services.AddMemoryCache();
    }
}
