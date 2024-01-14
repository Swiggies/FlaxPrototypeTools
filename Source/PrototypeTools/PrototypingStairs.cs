using System;
using System.Collections.Generic;
using FlaxEngine;

namespace PrototypeTools
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
        [Serialize] private float height = 100;
        [Serialize] private int steps = 10;
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
            get => height; set
            {
                height = value;
                UpdateMesh(_tempModel.LODs[0].Meshes[0]);
            }
        }

        [Limit(2, 255)]
        [NoSerialize]
        public int Steps
        {
            get => steps; set
            {
                steps = value;
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

            GenerateBase();
            GenerateSteps();
        }

        private void GenerateBase()
        {
            _vertices = new List<Float3>()
            {
                // Base
                new Float3(Width, 0, Depth),
                new Float3(0, 0, Depth),
                new Float3(0, 0, 0),
                new Float3(Width, 0, 0),

                // Right
                new Float3(Width, 0, 0),
                new Float3(Width, Height, Depth),
                new Float3(Width, 0, Depth),
                new Float3(Width, 0, 0),

                // Left
                new Float3(0, 0, 0),
                new Float3(0, 0, Depth),
                new Float3(0, Height, Depth),
                new Float3(0, 0, 0),

                // Front
                new Float3(Width, Height, Depth),
                new Float3(0, Height, Depth),
                new Float3(0, 0, Depth),
                new Float3(Width, 0, Depth),
            };

            _triangles = new List<int>
            {
                0,1,2,
                2,3,0,

                4,5,6,
                6,7,4,

                8,9,10,
                10,11,8,

                12,13,14,
                14,15,12,
            };

            triCount += 16;

            _normals = new List<Float3>()
            {
                -Float3.Up,
                -Float3.Up,
                -Float3.Up,
                -Float3.Up,

                Float3.Right,
                Float3.Right,
                Float3.Right,
                Float3.Right,

                Float3.Left,
                Float3.Left,
                Float3.Left,
                Float3.Left,

                Float3.Forward,
                Float3.Forward,
                Float3.Forward,
                Float3.Forward,
            };

            _uvs = new List<Float2>()
            {
                // Base
                new Float2(Width * 0.01f, Depth * 0.01f),
                new Float2(0, Depth * 0.01f),
                new Float2(0, 0),
                new Float2(Width * 0.01f, 0),

                // Right
                new Float2(0,0),
                new Float2(Depth * 0.01f, Height * 0.01f),
                new Float2(Depth * 0.01f, 0),
                new Float2(0, Height * 0.01f),

                // Left
                new Float2(0,0),
                new Float2(Depth * 0.01f, 0),
                new Float2(Depth * 0.01f, Height * 0.01f),
                new Float2(0, Height * 0.01f),

                // Front
                new Float2(Width * 0.01f, Height * 0.01f),
                new Float2(0, Height * 0.01f),
                new Float2(0,0),
                new Float2(Width * 0.01f, 0),
            };
        }

        private void GenerateSteps()
        {
            for (int i = 0; i < Steps; i++)
            {
                var pos = Vector3.Lerp(Vector3.Zero, new Vector3(0, Height, Depth), (float)i / Steps);
                var stepHeight = Height * (1f / Steps);
                var stepDepth = Depth * (1f / Steps);
                var stepP = (1.0f / Steps) * 0.01f;
                var curStep = Mathf.Clamp(((float)i / (Steps)), 0f, 1f);
                var nextStep = ((float)(i+1) / (Steps));

                for (int j = 0; j < 2; j++)
                {
                    _vertices.AddRange(new List<Float3>()
                    {
                        // Front of Step
                        new Float3(pos),
                        new Float3(pos.X, pos.Y + stepHeight, pos.Z),
                        new Float3(Width, pos.Y + stepHeight, pos.Z),
                        new Float3(Width, pos.Y, pos.Z),

                        // Top of Step
                        new Float3(pos.X, pos.Y + stepHeight, pos.Z),
                        new Float3(pos.X, pos.Y + stepHeight, pos.Z + stepDepth),
                        new Float3(Width, pos.Y + stepHeight, pos.Z + stepDepth),
                        new Float3(Width, pos.Y + stepHeight, pos.Z),

                        // Right Step
                        new Float3(Width, pos.Y, pos.Z),
                        new Float3(Width, pos.Y + stepHeight, pos.Z),
                        new Float3(Width, pos.Y + stepHeight, pos.Z + stepDepth),
                        new Float3(Width, pos.Y, pos.Z),

                        // Left Step
                        new Float3(pos.X, pos.Y, pos.Z),
                        new Float3(pos.X, pos.Y + stepHeight, pos.Z + stepDepth),
                        new Float3(pos.X, pos.Y + stepHeight, pos.Z),
                        new Float3(pos.X, pos.Y, pos.Z),
                    });

                    _triangles.AddRange(new List<int>()
                    {
                        triCount+0, triCount+1, triCount+2,
                        triCount+2, triCount+3, triCount+0,

                        triCount+4, triCount+5, triCount+6,
                        triCount+6, triCount+7, triCount+4,

                        triCount+8, triCount+9, triCount+10,
                        triCount+10, triCount+11, triCount+8,

                        triCount+12, triCount+13, triCount+14,
                        triCount+14, triCount+15, triCount+12,
                    });

                    triCount += 16;

                    _normals.AddRange(new List<Float3>()
                    {
                        Float3.Backward,
                        Float3.Backward,
                        Float3.Backward,
                        Float3.Backward,

                        Float3.Up,
                        Float3.Up,
                        Float3.Up,
                        Float3.Up,

                        Float3.Right,
                        Float3.Right,
                        Float3.Right,
                        Float3.Right,

                        Float3.Left,
                        Float3.Left,
                        Float3.Left,
                        Float3.Left,
                    });

                    _uvs.AddRange(new List<Float2>()
                    {
                        // Back
                        new Float2(0, 0),
                        new Float2(0 , Height * stepP),
                        new Float2(Width * 0.01f, Height * stepP),
                        new Float2(Width * 0.01f, 0),

                        // Top
                        new Float2(0, 0),
                        new Float2(0, Depth * stepP),
                        new Float2(Width * 0.01f, Depth * stepP),
                        new Float2(Width * 0.01f, 0),

                        // Right
                        new Float2(curStep * (Depth * 0.01f), curStep * (Height * 0.01f)),
                        new Float2(curStep * (Depth * 0.01f), nextStep * (Height * 0.01f)),
                        new Float2(nextStep * (Depth * 0.01f), nextStep * (Height * 0.01f)),
                        new Float2(nextStep * (Depth * 0.01f), curStep * (Height * 0.01f)),

                        // Left
                        new Float2(curStep * (Depth * 0.01f), curStep * (Height * 0.01f)),
                        new Float2(nextStep * (Depth * 0.01f), nextStep * (Height * 0.01f)),
                        new Float2(curStep * (Depth * 0.01f), nextStep * (Height * 0.01f)),
                        new Float2(nextStep * (Depth * 0.01f), curStep * (Height * 0.01f)),
                    });
                }
            }
        }
    }
}
