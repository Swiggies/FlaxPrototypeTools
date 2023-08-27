using System;
using System.Collections.Generic;
using FlaxEngine;
using FlaxEditor;
using System.Collections.Concurrent;

namespace PrototypeTools
{
    /// <summary>
    /// PrototypToolsPlugin Script.
    /// </summary>
    public class PrototypingToolsPlugin : EditorPlugin
    {
        public PrototypingToolsPlugin()
        {
            _description = new PluginDescription
            {
                Name = "Prototyping Tools",
            };
        }

        public override void DeinitializeEditor()
        {
            base.DeinitializeEditor();
        }

        public override void InitializeEditor()
        {
            base.InitializeEditor();
        }
    }
}
