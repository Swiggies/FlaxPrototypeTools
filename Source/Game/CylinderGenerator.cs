using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using FlaxEngine;

namespace Game
{
    /// <summary>
    /// BoxGenerator Script.
    /// </summary>

    [ExecuteInEditMode]
    public class CylinderGenerator : Script
    {
        [Limit(1)]
        public float Radius;
        [Limit(1)]
        public float Height;
        [Limit(3, 36)]
        public int Sides;

        public MaterialBase material;
        private Model _tempModel;
        private Mesh mesh;
        private float _angle;
        private MeshCollider _meshCollider;
        private CollisionData _collisionData;
        private Thread _thread;

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
            _tempModel = Content.CreateVirtualAsset<Model>();
            _tempModel.SetupLODs(new[] { 1 });
            UpdateMesh(_tempModel.LODs[0].Meshes[0]);

            var childModel = Actor.GetOrAddChild<StaticModel>();
            childModel.Model = _tempModel;
            childModel.SetMaterial(0, material);
            childModel.HideFlags = HideFlags.HideInHierarchy | HideFlags.DontSelect;
        }

        private void GenerateCylinder()
        {
            _vertices = new List<Float3>();
            _triangles = new List<int>();
            _normals = new List<Float3>();

            _angle = ((360.0f / (float)Sides) / 360.0f) * Mathf.Pi * 2;

            for (int i = 0; i < Sides; i++)
            {
                for (int u = 0; u < 4; u++)
                {
                    float y = (u % 2) * Height;
                    float x = Mathf.Cos(_angle * i) * Radius;
                    float z = Mathf.Sin(_angle * i) * Radius;
                    Float3 pos = new Float3(x, y, z);
                    _vertices.Add(pos);
                    if (u == 1) i++;
                    if (u == 3) i--;
                }

                var j = _vertices.Count - 4;
                _triangles.Add(j);
                _triangles.Add(j + 1);
                _triangles.Add(j + 3);

                _triangles.Add(j + 3);
                _triangles.Add(j + 2);
                _triangles.Add(j);

                var dir1 = (_vertices[j + 1] - _vertices[j]).Normalized;
                var dir2 = (_vertices[j + 2] - _vertices[j]).Normalized;
                _normals.Add(Vector3.Cross(dir1, dir2));

                dir1 = (_vertices[j] - _vertices[j + 1]).Normalized;
                dir2 = (_vertices[j + 3] - _vertices[j + 1]).Normalized;
                _normals.Add(-Vector3.Cross(dir1, dir2));

                dir1 = (_vertices[j] - _vertices[j + 2]).Normalized;
                dir2 = (_vertices[j + 3] - _vertices[j + 2]).Normalized;
                _normals.Add(Vector3.Cross(dir1, dir2));

                dir1 = (_vertices[j + 2] - _vertices[j + 3]).Normalized;
                dir2 = (_vertices[j + 1] - _vertices[j + 3]).Normalized;
                _normals.Add(Vector3.Cross(dir1, dir2));
            }
            MakeCircle(true);
            MakeCircle(false);
        }

        private void MakeCircle(bool top)
        {
            Float3 normal = top ? Float3.Up : Float3.Down;
            float height = top ? Height : 0;

            int indexCount = _vertices.Count;
            for (int i = 0; i < Sides; i++)
            {
                float y = height;
                float x = Mathf.Cos(_angle * i) * Radius;
                float z = Mathf.Sin(_angle * i) * Radius;
                Float3 pos = new Float3(x, y, z);
                _vertices.Add(pos);
                _normals.Add(normal);
            }
            _vertices.Add(new Float3(0, height, 0));
            _normals.Add(normal);

            if (top)
            {
                for (int i = indexCount; i < _vertices.Count - 1; i++)
                {
                    if (i == _vertices.Count - 2)
                    {
                        _triangles.Add(i);
                        _triangles.Add(_vertices.Count - 1);
                        _triangles.Add(indexCount);
                        break;
                    }
                    _triangles.Add(i);
                    _triangles.Add(_vertices.Count - 1);
                    _triangles.Add(i + 1);
                }
            }
            else
            {
                for (int i = indexCount; i < _vertices.Count - 1; i++)
                {
                    if (i == _vertices.Count - 2)
                    {
                        _triangles.Add(indexCount);
                        _triangles.Add(_vertices.Count - 1);
                        _triangles.Add(i);
                        break;
                    }
                    _triangles.Add(i + 1);
                    _triangles.Add(_vertices.Count - 1);
                    _triangles.Add(i);
                }
            }
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

        private void UpdateMesh(Mesh mesh)
        {
            GenerateCylinder();
            mesh.UpdateMesh(_vertices, _triangles, _normals);
            SetupCollision();
        }

        /// <inheritdoc/>
        public override void OnDisable()
        {
            // Here you can add code that needs to be called when script is disabled (eg. unregister from events)
        }

        /// <inheritdoc/>
        public override void OnUpdate()
        {
            UpdateMesh(_tempModel.LODs[0].Meshes[0]);
        }
    }
}
