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

    [ActorContextMenu("New/Prototyping/Cylinder")]
    [ActorToolbox("Prototyping")]
    public class PrototypingCylinder : PrototypingActor
    {
        [Serialize] private float radius = 50;
        [Serialize] private float height = 100;
        [Serialize] private int sides = 8;
        [Serialize] private float _angle;

        [Limit(1)]
        [NoSerialize]
        public float Radius { get => radius; set
            {
                radius = value;
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

        [Limit(3, 36)]
        [NoSerialize]
        public int Sides { get => sides; set
            {
                sides = value;
                UpdateMesh(_tempModel.LODs[0].Meshes[0]);
            }
        }

        protected override void GenerateModel()
        {
            _vertices = new List<Float3>();
            _triangles = new List<int>();
            _normals = new List<Float3>();
            _uvs = new List<Float2>();

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

                _uvs.Add(new Float2(0, 0));
                _uvs.Add(new Float2(0, Height * 0.01f));
                _uvs.Add(new Float2(1, 0));
                _uvs.Add(new Float2(1, Height * 0.01f));
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
                _uvs.Add(new Float2(0, 0));
            }
            _vertices.Add(new Float3(0, height, 0));
            _normals.Add(normal);
            _uvs.Add(Float2.One * (Radius * 0.01f));

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
    }
}
