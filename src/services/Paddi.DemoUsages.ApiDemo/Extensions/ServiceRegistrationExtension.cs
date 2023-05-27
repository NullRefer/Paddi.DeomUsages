using System.Reflection;

namespace Paddi.DemoUsages.ApiDemo.Extensions;

public static class ServiceRegistrationExtension
{
    public static IServiceCollection AddPaddiServices(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var typesToRegister = assembly.ExportedTypes.Where(t => t.IsInterface && t.IsAssignableTo(typeof(IAppService))).ToList();

        foreach (var typeToRegister in typesToRegister)
        {
            var implementations = assembly.ExportedTypes.Where(t => !t.IsAbstract && t.IsAssignableTo(typeToRegister));
            
            foreach (var implementation in implementations)
            {
                services.AddScoped(typeToRegister, implementation);
            }
        }

        return services;
    }
}
