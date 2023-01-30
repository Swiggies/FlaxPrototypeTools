using System;
using System.Collections.Generic;
using FlaxEditor.CustomEditors;
using FlaxEditor.CustomEditors.Editors;
using FlaxEngine;

namespace Game
{
    [ActorContextMenu("New/Prototyping/Ramp")]
    [ActorToolbox("Prototyping")]
    public class PrototypingRamp : PrototypingActor
    {
        [Serialize] private float width = 100f;
        [Serialize] private float depth = 100f;
        [Serialize] private float height = 100f;

        [ReadOnly] public float angle = 0;

        [Limit(1)]
        [NoSerialize]
        public float Width { get => width; set
            {
                width = value;
                UpdateMesh(_tempModel.LODs[0].Meshes[0]);
            }
        }

        [Limit(1)]
        [NoSerialize]
        public float Depth { get => depth; set
            {
                depth = value;
                UpdateMesh(_tempModel.LODs[0].Meshes[0]);
            }
        }

        [Limit(1)]
        [NoSerialize]
        public float Height { get => height; set
            {
                height = value;
                UpdateMesh(_tempModel.LODs[0].Meshes[0]);
            }
        }

        protected override void GenerateModel()
        {
            _vertices = new List<Float3>
            {
                // Front
                new Float3(Width, 0, Depth),
                new Float3(Width, Height, Depth),
                new Float3(0, Height, Depth),
                new Float3(0, 0, Depth),

                // Right
                new Float3(Width, 0, 0),
                new Float3(Width, Height, Depth),
                new Float3(Width, 0, Depth),
                new Float3(Width, 0, 0),

                // Left
                new Float3(0, 0, Depth),
                new Float3(0, Height, Depth),
                new Float3(0, 0, 0),
                new Float3(0, 0, 0),
                
                // Top
                new Float3(0,0,0),
                new Float3(0, Height, Depth),
                new Float3(Width, Height, Depth),
                new Float3(Width, 0, 0),

                // Bottom
                new Float3(0, 0, 0),
                new Float3(Width, 0, 0),
                new Float3(Width, 0, Depth),
                new Float3(0, 0, Depth),
            };

            _triangles = new List<int>
            {
                0, 1, 2, // Back
                2, 3, 0,

                // Right
                4, 5, 6,
                6, 7, 4,

                // Left,
                8, 9, 10,
                10, 11, 8,

                // Top
                12, 13, 14,
                14, 15, 12,

                // Bottom
                16, 17, 18,
                18, 19, 16,
            };

            _normals = new List<Float3>
            {
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
                // Front
                new Float2(1 * (Width * 0.01f), 0),
                new Float2(1 * (Width * 0.01f), 1 * (Height * 0.01f)),
                new Float2(0, 1 * (Height * 0.01f)),
                new Float2(0, 0),
                
                // Right
                new Float2(0, 0),
                new Float2(1 * (Depth * 0.01f), 1 * (Height * 0.01f)),
                new Float2(1 * (Depth * 0.01f), 0),
                new Float2(0, 0),

                // Left
                new Float2(1 * (Depth * 0.01f), 0),
                new Float2(1 * (Depth * 0.01f), 1 * (Height * 0.01f)),
                new Float2(0, 0),
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

            var dir = new Vector3(0, Height, Depth) - Vector3.Zero;
            angle = Mathf.Round(Vector3.Angle(Vector3.Forward, new Vector3(0, Height, Depth)));
        }
    }
}
