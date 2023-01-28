using System;
using System.Collections.Generic;
using FlaxEngine;

namespace Game
{
    /// <summary>
    /// BoxGenerator Script.
    /// </summary>

    [ExecuteInEditMode]
    public class BoxGenerator : Script
    {
        public float Width;
        public float Depth;
        public float Height;

        public MaterialBase material;
        private Model _tempModel;
        private Mesh mesh;
        private MeshCollider _meshCollider;
        private CollisionData _collisionData;

        List<Float3> _vertices;
        List<Float3> _normals;
        List<int> _triangles;
        List<Float2> _uvs;

        List<Color> _colors = new List<Color>
        {
            Color.Red,
            Color.Green,
            Color.Blue,
            Color.Yellow,
        };

        /// <inheritdoc/>
        public override void OnStart()
        {
            // Here you can add code that needs to be called when script is created, just before the first game update
        }
        
        /// <inheritdoc/>
        public override void OnEnable()
        {
            GenerateMesh();

            var model = Content.CreateVirtualAsset<Model>();
            _tempModel = model;
            model.SetupLODs(new[] { 1 });
            UpdateMesh(model.LODs[0].Meshes[0]);

            var childModel = Actor.GetOrAddChild<StaticModel>();
            childModel.Model = model;
            childModel.SetMaterial(0, material);
            childModel.HideFlags = HideFlags.HideInHierarchy | HideFlags.DontSelect;
        }

        private void UpdateMesh(Mesh mesh)
        {
            GenerateMesh();
            mesh.UpdateMesh(_vertices, _triangles, _normals, uv: _uvs);
            SetupCollision();
        }

        /// <inheritdoc/>
        public override void OnDisable()
        {
            // Here you can add code that needs to be called when script is disabled (eg. unregister from events)
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

        public void GenerateMesh()
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

        /// <inheritdoc/>
        public override void OnUpdate()
        {
            UpdateMesh(_tempModel.LODs[0].Meshes[0]);
        }
    }
}
