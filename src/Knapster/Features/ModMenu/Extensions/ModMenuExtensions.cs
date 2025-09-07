using Knapster.Features.ModMenu.Dialogue.Abstractions;
using Knapster.Features.ModMenu.Systems;
using System.Diagnostics.CodeAnalysis;

namespace Knapster.Features.ModMenu.Extensions;

/// <summary>
///     Provides extension methods for integrating features into the mod menu.
/// </summary>
internal static class ModMenuExtensions
{
    /// <summary>
    ///     Adds a mod settings dialogue.
    /// </summary>
    [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Extension Method")]
    public static void AddSettingsTab(this ICoreClientAPI capi, Func<ComposableGuiTab> factory) 
    {
        var system = G.Services.GetRequiredService<KnapsterAdminModMenu>();
        system.TabFactories.AddIfNotPresent(factory);
    }
}