using ApacheTech.Common.DependencyInjection.Abstractions.Extensions;

namespace XKnapster;

public sealed class Program() : ModHost<Program>(G.SetCore);

internal class Test : UniversalModSystem<Test>
{
    public override void Start(ICoreAPI api)
    {
        Core.ForMod("knapster")
            .Logger.Audit($"{GetHashCode()} This is a test message from {G.Mod.Info.Name}.");
    }
}