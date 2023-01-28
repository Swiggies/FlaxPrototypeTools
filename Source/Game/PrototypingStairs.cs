using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using FlaxEngine;
using System.Linq;
using System.Threading;

namespace Game
{
    /// <summary>
    /// BoxGenerator Script.
    /// </summary>

    [ActorContextMenu("New/Prototyping/Stairs")]
    [ActorToolbox("Prototyping")]
    public class PrototypingStairs : PrototypingActor
    {
        [Serialize] private float _width = 100;  
        [Serialize] private float _depth = 100;
        [Serialize] private float _height = 100;
        [Serialize] private int _steps = 10;
        int triCount;

        [Limit(1)]
        [NoSerialize]
        public float Width
        {
            get => _width; set
            {
                _width = value;
                UpdateMesh(_tempModel.LODs[0].Meshes[0]);
            }
        }

        [Limit(1)]
        [NoSerialize]
        public float Depth
        {
            get => _depth; set
            {
                _depth = value;
                UpdateMesh(_tempModel.LODs[0].Meshes[0]);
            }
        }

        [Limit(1)]
        [NoSerialize]
        public float Height
        {
            get => _height; set
            {
                _height = value;
                UpdateMesh(_tempModel.LODs[0].Meshes[0]);
            }
        }

        [Limit(2)]
        [NoSerialize]
        public int Steps
        {
            get => _steps; set
            {
                _steps = value;
                UpdateMesh(_tempModel.LODs[0].Meshes[0]);
            }
        }
        protected override void GenerateModel()
        {
            triCount = 0;
            _triangles = new List<int>();
            _vertices = new List<Float3>();
            _normals = new List<Float3>();
            _uvs = new List<Float2>();

            for (int i = 1; i <= Steps; i++)
            {
                GenerateStep(i, triCount);
            }
        }

        private List<Float3> GetVertices(int step)
        {
            float heightPercent = ((float)step / (float)Steps);
            float prevHeightPercent = (step - 1f) / (float)Steps;

            return new List<Float3>
            {
                // Back
                new Float3(0, 0, Depth * prevHeightPercent),
                new Float3(0, Height * heightPercent, Depth * prevHeightPercent),
                new Float3(Width, Height * heightPercent, Depth * prevHeightPercent),
                new Float3(Width, 0,  Depth * prevHeightPercent),

                // Front
                new Float3(Width, 0, Depth * heightPercent),
                new Float3(Width, Height * heightPercent, Depth * heightPercent),
                new Float3(0, Height * heightPercent, Depth * heightPercent),
                new Float3(0, 0, Depth * heightPercent),

                // Right
                new Float3(Width, 0, Depth * prevHeightPercent),
                new Float3(Width, Height * heightPercent, Depth * prevHeightPercent),
                new Float3(Width, Height * heightPercent, Depth * heightPercent),
                new Float3(Width, 0, Depth * heightPercent),

                // Left
                new Float3(0, 0, Depth * heightPercent),
                new Float3(0, Height * heightPercent, Depth * heightPercent),
                new Float3(0, Height * heightPercent, Depth * prevHeightPercent),
                new Float3(0, 0, Depth * prevHeightPercent),

                // Top
                new Float3(0, Height * heightPercent, Depth * prevHeightPercent),
                new Float3(0, Height * heightPercent, Depth * heightPercent),
                new Float3(Width, Height * heightPercent, Depth * heightPercent),
                new Float3(Width, Height * heightPercent, Depth * prevHeightPercent),

                // Bottom
                new Float3(0, 0, Depth * prevHeightPercent),
                new Float3(Width, 0, Depth * prevHeightPercent),
                new Float3(Width, 0, Depth * heightPercent),
                new Float3(0, 0, Depth * heightPercent),
            };
        }

        private void GenerateStep(int step, int startIndex)
        {
            _vertices.AddRange(GetVertices(step));

            _triangles.AddRange(new List<int>
            {
                startIndex + 0, startIndex + 1, startIndex + 2, // Back
                startIndex + 2, startIndex + 3, startIndex + 0,

                startIndex + 4, startIndex + 5, startIndex + 6, // Front
                startIndex + 6, startIndex + 7, startIndex + 4,

                startIndex + 8, startIndex + 9, startIndex + 10, // Right
                startIndex + 10, startIndex + 11, startIndex + 8,

                startIndex + 12, startIndex + 13, startIndex + 14,
                startIndex + 14, startIndex + 15, startIndex + 12,

                startIndex + 16, startIndex + 17, startIndex + 18,
                startIndex + 18, startIndex + 19, startIndex + 16,

                startIndex + 20, startIndex + 21, startIndex + 22,
                startIndex + 22, startIndex + 23, startIndex + 20,
            });
            triCount += 24;

            _normals.AddRange(new List<Float3>
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
            });

            _uvs.AddRange(new List<Float2>
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
            });
        }
    }
}
