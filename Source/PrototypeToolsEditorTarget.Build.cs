using Flax.Build;

public class PrototypeToolsEditorTarget : GameProjectEditorTarget
{
    /// <inheritdoc />
    public override void Init()
    {
        base.Init();

        // Reference the modules for editor
        Modules.Add("PrototypeTools");
    }
}
