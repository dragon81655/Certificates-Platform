using ServiceRegistration;
using System.Reflection;
namespace Certificates_Platform
{
    public interface IConfigurationLoader
    {
        public static abstract void LoadSettings(WebApplicationBuilder builder);
    }
    public class GenerationSettings : IConfigurationLoader
    {

        public int maximumAmountOfProcesses { get; set; }
        public int forceScanvanger { get; set; }

        public static void LoadSettings(WebApplicationBuilder builder)
        {
            builder.Services.Configure<GenerationSettings>(builder.Configuration.GetSection("GenerationSettings"));
        }
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
