using System.Reflection;
namespace ServiceRegistration
{
    public class FormSiteData : IConfigurationLoader
    {
        public string key { get; set; }
        public string link { get; set; }

        public static void LoadSettings(WebApplicationBuilder builder)
        {
            builder.Services.Configure<FormSiteData>(builder.Configuration.GetSection("FormSiteData"));
        }
    }

    public interface IConfigurationLoader
    {
        public static abstract void LoadSettings(WebApplicationBuilder builder);
    }
    public class RegisterOptions
    {
        public void Compose(WebApplicationBuilder builder, Assembly assembly)
        {
            var configTypes = assembly.GetTypes()
          .Where(t => typeof(IConfigurationLoader).IsAssignableFrom(t)
                   && t.IsClass && !t.IsAbstract)
          .ToList();

            foreach (var type in configTypes)
            {
                type.GetMethod("LoadSettings", BindingFlags.Public | BindingFlags.Static)
                    ?.Invoke(null, new object[] { builder });
            }

        }
    }
}
