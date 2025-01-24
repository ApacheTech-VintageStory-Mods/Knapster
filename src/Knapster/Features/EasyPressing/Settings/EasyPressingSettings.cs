using Gantry.Services.FileSystem.Configuration.Abstractions;
using ProtoBuf;

namespace ApacheTech.VintageMods.Knapster.Features.EasyPressing.Settings;

[ProtoContract]
[ProtoInclude(100, typeof(EasyPressingClientSettings))]
public class EasyPressingSettings : FeatureSettings<EasyPressingServerSettings>;