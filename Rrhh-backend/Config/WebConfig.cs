using System.Text.Json.Serialization;

namespace Rrhh_backend.Config
{
    public static class WebConfig
    {
        public static void AddCustomWebConfiguration(this IServiceCollection services) {
            services.ConfigureHttpJsonOptions(options =>
            {
                options.SerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
                options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
        }
    }
}
