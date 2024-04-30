using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace WebAPI.Extensions;

internal static class JsonSerializerSettingExtensions
{
    internal static void AddNewtonsoftJsonSerializerSettings(this IServiceCollection services)
    {
        JsonSerializerSettings settings = new()
        {
            TypeNameHandling = TypeNameHandling.All,
            ContractResolver = new DefaultContractResolver()
            {
                NamingStrategy = new CamelCaseNamingStrategy(),
                SerializeCompilerGeneratedMembers = true,
            },
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };

        JsonConvert.DefaultSettings = () => settings;
    }
}