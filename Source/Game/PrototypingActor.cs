using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using FlaxEngine;

namespace Game
{
    /// <summary>
    /// PrototypingActor Script.
    /// </summary>
    public class PrototypingActor : Actor
    {
        [Serialize] private MaterialBase _material;
        protected Model _tempModel;
        protected MeshCollider _meshCollider;
        protected CollisionData _collisionData;
        private StaticModel _staticModel;

        protected List<Float3> _vertices;
        protected List<Float3> _normals;
        protected List<int> _triangles;
        protected List<Float2> _uvs;

        private float _timer = 0f;
        private bool _needsBaking = true;

        [NoSerialize]
        public MaterialBase Material { get => _material; set
            {
                _material = value;
                _staticModel.SetMaterial(0, _material);
            }
        }

        public override void OnEnable()
        {
            base.OnEnable();
            _tempModel = Content.CreateVirtualAsset<Model>();
            _tempModel.SetupLODs(new[] { 1 });
            UpdateMesh(_tempModel.LODs[0].Meshes[0]);

            _material = Content.Load<MaterialBase>(Guid.Parse("4fae8dc84a46b69d87adf5bf2c050821"));

            _staticModel = GetOrAddChild<StaticModel>();
            _staticModel.HideFlags = HideFlags.HideInHierarchy | HideFlags.DontSelect;
            _staticModel.Model = _tempModel;
            _staticModel.SetMaterial(0, _material);

            Scripting.Update += UpdateTimer;
        }

        private void UpdateTimer()
        {
            _timer += Time.UnscaledDeltaTime;
            if(_timer >= 1.0f && _needsBaking)
            {
                _needsBaking = false;
                BakeCollisionSDF();
            }
        }

        protected virtual void GenerateModel() { }

        protected virtual void UpdateMesh(Mesh mesh)
        {
            GenerateModel();
            mesh.UpdateMesh(_vertices, _triangles, _normals, uv: _uvs);

            _needsBaking = true;
            _timer = 0.0f;
        }

        protected virtual void BakeCollisionSDF()
        {
            if (_tempModel == null) return;
            if (_tempModel.IsVirtual)
            {
                _collisionData = Content.CreateVirtualAsset<CollisionData>();
                var colJob = JobSystem.Dispatch(i =>
                {
                    _collisionData.CookCollision(CollisionDataType.TriangleMesh, vertices: _vertices.ToArray(), triangles: _triangles.ToArray());
                    _meshCollider = GetOrAddChild<MeshCollider>();
                    _meshCollider.HideFlags = HideFlags.HideInHierarchy | HideFlags.DontSelect;
                    _meshCollider.CollisionData = _collisionData;
                    //_tempModel.GenerateSDF();
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
