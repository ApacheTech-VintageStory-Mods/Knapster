using System.ComponentModel;

namespace Knapster.Features.ModMenu.Packets;

[ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
public class SettingsSavedPacket
{
    [DefaultValue("")]
    public string FeatureName { get; set; } = string.Empty;
}