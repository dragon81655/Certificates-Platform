using Certificates_Platform;
using System.Reflection;

namespace ServiceRegistration
{
    public class MyRegisteringAttribute : Attribute
    {
    }

    public class RegisterAsSingletonAttribute : MyRegisteringAttribute
    {

    }
    public class RegisterAsScopedAttribute : MyRegisteringAttribute
    {

    }
    public class RegisterAsTransientAttribute : MyRegisteringAttribute
    {

    }
    public class RegisterAsServiceAttribute : MyRegisteringAttribute
    {

    }
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class OptionsConfigAttribute : Attribute
    {
        public string SectionName { get; }

        public OptionsConfigAttribute(string sectionName)
        {
            SectionName = sectionName;
        }
    }
    public class RegisterServices
    {
        public RegisterServices(WebApplicationBuilder builder, Assembly assembly) 
        {
            RegisterAllServices(builder.Services, assembly);
        }
        

        private void RegisterAllServices(IServiceCollection services, Assembly assembly)
        {
            List<Type> types = assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.GetCustomAttributes<MyRegisteringAttribute>().Any())
                .ToList();
            for (int i = 0; i < types.Count; i++)
            {
                var attr = types[i].GetCustomAttributes<MyRegisteringAttribute>(inherit: true)
                   .FirstOrDefault();
                if (attr == null) continue;
                switch (attr)
                {
                    case RegisterAsSingletonAttribute:
                        services.AddSingleton(types[i]);
                        break;
                    case RegisterAsScopedAttribute:
                        services.AddScoped(types[i]);
                        break;
                    case RegisterAsTransientAttribute:
                        services.AddTransient(types[i]);
                        break;
                    case RegisterAsServiceAttribute:
                        var interfaces = types[i].GetInterfaces();
                        foreach (var iface in interfaces)
                        {
                            if (typeof(IHostedService).IsAssignableFrom(types[i]))
                            {
                                services.AddSingleton(typeof(IHostedService), types[i]);
                                break;
                            }
                        }
                        break;
                }
            }
        }
        
    }
    public interface IComposer
    {
        public void Compose(WebApplicationBuilder builder, Assembly assembly);
    }
}