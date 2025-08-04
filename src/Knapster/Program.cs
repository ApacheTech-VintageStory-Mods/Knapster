using ApacheTech.Common.DependencyInjection.Abstractions.Extensions;

namespace Knapster;

internal class Program() : EasyXHost<Program>("knapster", G.SetCore);

internal class Test : UniversalModSystem<Test>
{
    public override void Start(ICoreAPI api)
    {
        Core.ForMod("xknapster")
            .Logger.Audit($"{GetHashCode()} This is a test message from {G.Mod.Info.Name}.");
    }
}