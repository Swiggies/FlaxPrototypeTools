using System;
using System.Collections.Generic;
using FlaxEngine;

namespace PrototypeTools
{
    /// <summary>
    /// PrototypingActor Script.
    /// </summary>
    public class PrototypingActor : Actor
    {
        [Serialize, HideInEditor] public Material _material;
        [NoSerialize] protected Model _tempModel;
        [NoSerialize] protected MeshCollider _meshCollider;
        [NoSerialize] protected CollisionData _collisionData;

        protected List<Float3> _vertices;
        protected List<Float3> _normals;
        protected List<int> _triangles;
        protected List<Float2> _uvs;
        protected List<Color32> _colors;
        private float _timer = 0f;

        [Serialize] private bool _needsBaking = false;

        [NoSerialize]
        public Material ModelMaterial
        {
            get => _material;
            set
            {
                _material = value;
                GetOrAddChild<StaticModel>().SetMaterial(0, _material);
            }
        }

        public override void OnEnable()
        {
            base.OnEnable();
            _tempModel = Content.CreateVirtualAsset<Model>();
            _tempModel.SetupLODs(new[] { 1 });
            UpdateMesh(_tempModel.LODs[0].Meshes[0]);

            //if(_material == null)
            //    _material = Content.Load<MaterialBase>(FlaxEngine.Json.JsonSerializer.ParseID("4fae8dc84a46b69d87adf5bf2c050821"));

            var staticModel = GetOrAddChild<StaticModel>();
            staticModel.HideFlags = HideFlags.HideInHierarchy | HideFlags.DontSelect;
            staticModel.Model = _tempModel;
            staticModel.SetMaterial(0, _material);
            //Scripting.Update += UpdateTimer;
        }

        public override void OnDisable()
        {
            Destroy(ref _tempModel);
            Destroy(ref _collisionData);

            base.OnDisable();
            //Scripting.Update -= UpdateTimer;
        }

        private void UpdateTimer()
        {
            _timer += Time.UnscaledDeltaTime;
            if (_timer >= 1.0f && _needsBaking)
            {
                _needsBaking = false;
            }
        }

        protected virtual void GenerateModel() { }

        protected virtual void UpdateMesh(Mesh mesh)
        {
            GenerateModel();
            mesh.UpdateMesh(_vertices, _triangles, _normals, uv: _uvs, colors: _colors);
            BakeCollisionSDF();
            _needsBaking = true;
            _timer = 0.0f;
        }

        protected virtual void BakeCollisionSDF()
        {
            if (_tempModel == null) return;
            if (_tempModel.IsVirtual)
            {
                _collisionData = Content.CreateVirtualAsset<CollisionData>();
                _meshCollider = GetOrAddChild<MeshCollider>();
                _meshCollider.HideFlags = HideFlags.HideInHierarchy | HideFlags.DontSelect;
                var label = JobSystem.Dispatch(i =>
                {
                    if (!_collisionData.CookCollision(CollisionDataType.ConvexMesh, vertices: _vertices.ToArray(), triangles: _triangles.ToArray()))
                    {
                    }
                    else
                    {
                        Debug.LogError("Failed to generate colliders.");
                    }
                    //if(_tempModel.GenerateSDF())
                    //    Debug.LogError("Failed to generate SDF.");
                });
                JobSystem.Wait(label);
                _meshCollider.CollisionData = _collisionData;
            }
        }
    }
}
