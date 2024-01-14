using System;
using System.Collections.Generic;
using FlaxEngine;

namespace PrototypeTools
{
    /// <summary>
    /// PrototypeToolsPlugin Script.
    /// </summary>
    public class PrototypingToolsPlugin : GamePlugin
    {
        public PrototypingToolsPlugin()
        {
            _description = new PluginDescription
            {
                Name = "Prototyping Tools",
                Author = "Swiggies",
                Category = "Other",
                Description = "Easily editable whiteboxing tools.",
                RepositoryUrl = "https://github.com/Swiggies/FlaxPrototypeTools/",
            };
        }
    }
}
