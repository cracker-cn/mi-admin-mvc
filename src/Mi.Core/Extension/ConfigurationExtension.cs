using Microsoft.Extensions.Configuration;

namespace Mi.Core.Extension
{
    public static class ConfigurationExtension
    {
        public static T Bind<T>(this IConfiguration configuration, string sectionKey)
        {
            var section = configuration.GetSection(sectionKey);
            var model = Activator.CreateInstance<T>();
            if (section == null) return model;
            var children = section.GetChildren();
            foreach (var item in typeof(T).GetProperties())
            {
                var child = children.FirstOrDefault(x => x.Key == item.Name);
                if (child == null) continue;
                item.SetValue(model, Convert.ChangeType(child.Value,item.PropertyType));
            }
            return model;
        }

        public static IConfiguration AppSettings => new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json").Build();
    }
}
