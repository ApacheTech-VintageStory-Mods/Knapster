namespace Knapster.Features.EasyPressing.Settings;

[ProtoContract]
[ProtoInclude(100, typeof(EasyPressingClientSettings))]
public class EasyPressingSettings : FeatureSettings<EasyPressingServerSettings>;