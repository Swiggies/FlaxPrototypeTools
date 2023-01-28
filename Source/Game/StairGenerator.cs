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

    [ExecuteInEditMode]
    public class StairGenerator : Script
    {
        [Limit(1)]
        public float Width;
        [Limit(1)]
        public float Depth;
        [Limit(1)]
        public float Height;
        [Limit(1)]
        public int Steps;

        public MaterialBase material;
        private Model _tempModel;
        private Mesh mesh;
        private MeshCollider _meshCollider;
        private CollisionData _collisionData;
        private Thread _thread;

        List<Float3> _vertices;
        List<Float3> _normals;
        List<int> _triangles;
        List<Float2> _uvs;
        int triCount;

        List<Color> _colors = new List<Color>
        {
            Color.Red,
            Color.Green,
            Color.Blue,
            Color.Yellow,
        };

        /// <inheritdoc/>
        public override void OnEnable()
        {
            GenerateModel();

            _tempModel = Content.CreateVirtualAsset<Model>();
            _tempModel.SetupLODs(new[] { 1 });
            UpdateMesh(_tempModel.LODs[0].Meshes[0]);

            var childModel = Actor.GetOrAddChild<StaticModel>();
            childModel.HideFlags= HideFlags.HideInHierarchy | HideFlags.DontSelect;
            childModel.Model = _tempModel;
            childModel.SetMaterial(0, material);
        }

        private void UpdateMesh(Mesh mesh)
        {
            GenerateModel();
            mesh.UpdateMesh(_vertices, _triangles, _normals, uv: _uvs);
            SetupCollision();
        }

        /// <inheritdoc/>
        public override void OnDisable()
        {
            // Here you can add code that needs to be called when script is disabled (eg. unregister from events)
        }

        private void GenerateModel()
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

        private void UpdateModel()
        {
            //_vertices = GetVertices();
        }

        private void SetupCollision()
        {
            if (_tempModel == null) return;
            if (_tempModel.IsVirtual)
            {
                _collisionData = Content.CreateVirtualAsset<CollisionData>();
                JobSystem.Dispatch(i => {
                    _collisionData.CookCollision(CollisionDataType.TriangleMesh, vertices: _vertices.ToArray(), triangles: _triangles.ToArray());
                    _meshCollider = Actor.GetOrAddChild<MeshCollider>();
                    _meshCollider.HideFlags = HideFlags.HideInHierarchy | HideFlags.DontSelect;
                    _meshCollider.CollisionData = _collisionData;
                });
            }
        }

        /// <inheritdoc/>
        public override void OnUpdate()
        {
            UpdateMesh(_tempModel.LODs[0].Meshes[0]);
        }
    }
}
