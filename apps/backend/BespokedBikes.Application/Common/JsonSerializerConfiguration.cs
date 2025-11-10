using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BespokedBikes.Application.Common;

public static class JsonSerializerConfiguration
{
    public static void ConfigureNewtonsoftJson(JsonSerializerSettings settings) => settings.Converters.Add(new StringEnumConverter());
}
