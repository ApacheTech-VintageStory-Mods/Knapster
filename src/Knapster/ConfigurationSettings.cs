using Gantry.Services.FileSystem.Configuration.Abstractions;
using Gantry.Services.FileSystem.Enums;
using Newtonsoft.Json.Converters;

namespace ApacheTech.VintageMods.Knapster;

[JsonObject]
public class ConfigurationSettings : FeatureSettings
{
    [JsonConverter(typeof(StringEnumConverter))]
    public FileScope Scope { get; set; } = FileScope.World;
}