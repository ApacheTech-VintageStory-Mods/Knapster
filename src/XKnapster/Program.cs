namespace XKnapster;

public sealed class Program() : ModHost<Program>()
{
    protected sealed override void OnCoreLoaded(ICoreGantryAPI core) => G.SetCore(core);

    protected override void OnCoreUnloaded() => G.Dispose();
}