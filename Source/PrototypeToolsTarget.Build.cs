using Flax.Build;

public class PrototypeToolsTarget : GameProjectTarget
{
    /// <inheritdoc />
    public override void Init()
    {
        base.Init();

        // Reference the modules for game
        Modules.Add("PrototypeTools");
    }
}
