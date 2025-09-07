namespace Knapster;

internal sealed class Program() : EasyXHost<Program>("knapster")
{
    protected sealed override void OnCoreLoaded(ICoreGantryAPI core) => G.SetCore(core);

    protected override void OnCoreUnloaded() => G.Dispose();
}