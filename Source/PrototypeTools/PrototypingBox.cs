using System;
using System.Collections.Generic;
using FlaxEngine;

namespace PrototypeTools
{
    /// <summary>
    /// BoxGenerator Script.
    /// </summary>

    [ActorContextMenu("New/Prototyping/Box")]
    [ActorToolbox("Prototyping")]
    public class PrototypingBox : PrototypingActor
    {
        [Serialize]
        private float _width = 100f;
        [Serialize]
        private float _depth = 100f;
        [Serialize]
        private float _height = 100f;

        [Limit(1)]
        [NoSerialize]
        public float Width { get => _width; set 
            {
                _width = value;
                UpdateMesh(_tempModel.LODs[0].Meshes[0]);
            }
        }

        [Limit(1)]
        [NoSerialize]
        public float Depth { get => _depth; set
            {
                _depth = value;
                UpdateMesh(_tempModel.LODs[0].Meshes[0]);
            }
        }

        [Limit(1)]
        [NoSerialize]
        public float Height { get => _height; set
            {
                _height = value;
                UpdateMesh(_tempModel.LODs[0].Meshes[0]);
            }
        }

        protected override void GenerateModel()
        {
            _vertices = new List<Float3>
            {
                new Float3(0, 0, 0),
                new Float3(0, Height, 0),
                new Float3(Width, Height, 0),
                new Float3(Width, 0, 0),

                new Float3(Width, 0, Depth),
                new Float3(Width, Height, Depth),
                new Float3(0, Height, Depth),
                new Float3(0, 0, Depth),

                new Float3(Width, 0, 0),
                new Float3(Width, Height, 0),
                new Float3(Width, Height, Depth),
                new Float3(Width, 0, Depth),

                new Float3(0, 0, Depth),
                new Float3(0, Height, Depth),
                new Float3(0, Height, 0),
                new Float3(0, 0, 0),

                new Float3(0, Height, 0),
                new Float3(0, Height, Depth),
                new Float3(Width, Height, Depth),
                new Float3(Width, Height, 0),

                new Float3(0, 0, 0),
                new Float3(Width, 0, 0),
                new Float3(Width, 0, Depth),
                new Float3(0, 0, Depth),
            };

            _triangles = new List<int>
            {
                0, 1, 2, // Back
                2, 3, 0,

                4, 5, 6, // Front
                6, 7, 4,

                8, 9, 10, // Right
                10, 11, 8,

                12, 13, 14,
                14, 15, 12,

                16, 17, 18,
                18, 19, 16,

                20, 21, 22,
                22, 23, 20,
            };

            _normals = new List<Float3>
            {
                -Vector3.Forward,
                -Vector3.Forward,
                -Vector3.Forward,
                -Vector3.Forward,

                Vector3.Forward,
                Vector3.Forward,
                Vector3.Forward,
                Vector3.Forward,

                Vector3.Right,
                Vector3.Right,
                Vector3.Right,
                Vector3.Right,

                -Vector3.Right,
                -Vector3.Right,
                -Vector3.Right,
                -Vector3.Right,

                Vector3.Up,
                Vector3.Up,
                Vector3.Up,
                Vector3.Up,

                -Vector3.Up,
                -Vector3.Up,
                -Vector3.Up,
                -Vector3.Up,
            };

            _uvs = new List<Float2>
            {
                // Back
                new Float2(0, 0),
                new Float2(0, 1 * (Height * 0.01f)),
                new Float2(1 * (Width * 0.01f), 1 * (Height * 0.01f)),
                new Float2(1 * (Width * 0.01f), 0),

                // Front
                new Float2(1 * (Width * 0.01f), 0),
                new Float2(1 * (Width * 0.01f), 1 * (Height * 0.01f)),
                new Float2(0, 1 * (Height * 0.01f)),
                new Float2(0, 0),
                
                // Right
                new Float2(0, 0),
                new Float2(0, 1 * (Height * 0.01f)),
                new Float2(1 * (Depth * 0.01f), 1 * (Height * 0.01f)),
                new Float2(1 * (Depth * 0.01f), 0),

                // Left
                new Float2(1 * (Depth * 0.01f), 0),
                new Float2(1 * (Depth * 0.01f), 1 * (Height * 0.01f)),
                new Float2(0, 1 * (Height * 0.01f)),
                new Float2(0, 0),

                // Up
                new Float2(0, 0),
                new Float2(0, 1 * (Depth * 0.01f)),
                new Float2(1 * (Width * 0.01f), 1 * (Depth * 0.01f)),
                new Float2(1 * (Width * 0.01f), 0),

                // Down
                new Float2(0, 0),
                new Float2(1 * (Width * 0.01f), 0),
                new Float2(1 * (Width * 0.01f), 1 * (Depth * 0.01f)),
                new Float2(0, 1 * (Depth * 0.01f)),
            };
        }
    }
}
