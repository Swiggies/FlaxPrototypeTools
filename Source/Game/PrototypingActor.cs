using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using FlaxEngine;

namespace Game
{
    /// <summary>
    /// PrototypingActor Script.
    /// </summary>
    public class PrototypingActor : Actor
    {
        public MaterialBase material;
        protected Model _tempModel;
        protected MeshCollider _meshCollider;
        protected CollisionData _collisionData;

        protected List<Float3> _vertices;
        protected List<Float3> _normals;
        protected List<int> _triangles;
        protected List<Float2> _uvs;

        public override void OnEnable()
        {
            base.OnEnable();
            _tempModel = Content.CreateVirtualAsset<Model>();
            _tempModel.SetupLODs(new[] { 1 });
            UpdateMesh(_tempModel.LODs[0].Meshes[0]);

            var childModel = GetOrAddChild<StaticModel>();
            childModel.HideFlags = HideFlags.HideInHierarchy | HideFlags.DontSelect;
            childModel.Model = _tempModel;
            childModel.SetMaterial(0, material);
        }

        protected virtual void GenerateModel() { }

        protected virtual void UpdateMesh(Mesh mesh)
        {
            GenerateModel();
            mesh.UpdateMesh(_vertices, _triangles, _normals, uv: _uvs);
            SetupCollision();
        }

        protected virtual void SetupCollision()
        {
            if (_tempModel == null) return;
            if (_tempModel.IsVirtual)
            {
                _collisionData = Content.CreateVirtualAsset<CollisionData>();
                JobSystem.Dispatch(i => {
                    _collisionData.CookCollision(CollisionDataType.TriangleMesh, vertices: _vertices.ToArray(), triangles: _triangles.ToArray());
                    _meshCollider = GetOrAddChild<MeshCollider>();
                    _meshCollider.HideFlags = HideFlags.HideInHierarchy | HideFlags.DontSelect;
                    _meshCollider.CollisionData = _collisionData;
                });
            }
        }

        [OnSerializing]
        internal void OnSerializing(StreamingContext context)
        {
            UpdateMesh(_tempModel.LODs[0].Meshes[0]);
        }
    }
}
