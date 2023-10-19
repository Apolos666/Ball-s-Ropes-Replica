using UnityEngine;

namespace Apolos.Utils
{
    public class MeshDataRopes 
    {
        private Vector3[] _vertices;
        private int[] _triangles;
        private Vector2[] _uvs;
        private Vector3[] _bakedNormals;

        private int _triangleIndex = 0;

        private bool _useFlatShading;

        public MeshDataRopes(int numVertsPerCircle, int numVertsAlongRope, bool useFlatShading)
        {
            this._useFlatShading = useFlatShading;

            _vertices = new Vector3[numVertsPerCircle * numVertsAlongRope];
            _uvs = new Vector2[_vertices.Length];

            int numMainTriangles = (numVertsPerCircle * (numVertsAlongRope - 1)) * 2;
            _triangles = new int[numMainTriangles * 3];
            _triangleIndex = 0;
        }

        public void AddVertex(Vector3 vertexPosition, Vector2 uv, int vertexIndex)
        {
            _vertices[vertexIndex] = vertexPosition;
            _uvs[vertexIndex] = uv;
        }

        public void AddTriangle(int a, int b, int c)
        {
            _triangles[_triangleIndex] = a;
            _triangles[_triangleIndex + 1] = b;
            _triangles[_triangleIndex + 2] = c;
            _triangleIndex += 3;
        }

        Vector3[] CalculateNormals()
        {
            Vector3[] vertexNormals = new Vector3[_vertices.Length];
            int triangleCount = _triangles.Length / 3;
            for (int i = 0; i < triangleCount; i++)
            {
                int normalTriangleIndex = i * 3;
                int vertexIndexA = _triangles[normalTriangleIndex];
                int vertexIndexB = _triangles[normalTriangleIndex + 1];
                int vertexIndexC = _triangles[normalTriangleIndex + 2];

                Vector3 triangleNormal = SurfaceNormalFromIndices(vertexIndexA, vertexIndexB, vertexIndexC);
                vertexNormals[vertexIndexA] += triangleNormal;
                vertexNormals[vertexIndexB] += triangleNormal;
                vertexNormals[vertexIndexC] += triangleNormal;
            }

            for (int i = 0; i < vertexNormals.Length; i++)
            {
                vertexNormals[i].Normalize();
            }

            return vertexNormals;
        }

        private Vector3 SurfaceNormalFromIndices(int vertexIndexA, int vertexIndexB, int vertexIndexC)
        {
            Vector3 pointA = _vertices[vertexIndexA];
            Vector3 pointB = _vertices[vertexIndexB];
            Vector3 pointC = _vertices[vertexIndexC];

            Vector3 sideAB = pointB - pointA;
            Vector3 sideAC = pointC - pointA;
            return Vector3.Cross(sideAB, sideAC).normalized;
        }

        public void ProcessMesh()
        {
            if (_useFlatShading)
            {
                FlatShading();
            }
            else
            {
                BakeNormals();
            }
        }

        private void BakeNormals()
        {
            _bakedNormals = CalculateNormals();
        }

        private void FlatShading()
        {
            Vector3[] flatShadedVertices = new Vector3[_triangles.Length];
            Vector2[] flatShadedUvs = new Vector2[_triangles.Length];

            for (int i = 0; i < _triangles.Length; i++)
            {
                flatShadedVertices[i] = _vertices[_triangles[i]];
                flatShadedUvs[i] = _uvs[_triangles[i]];
                _triangles[i] = i;
            }

            _vertices = flatShadedVertices;
            _uvs = flatShadedUvs;
        }

        public Mesh CreateMesh()
        {
            Mesh mesh = new Mesh();
            mesh.vertices = _vertices;

            mesh.triangles = _triangles;
            mesh.uv = _uvs;

            if (_useFlatShading)
            {
                mesh.RecalculateNormals();
            }
            else
            {
                mesh.normals = _bakedNormals;
            }

            return mesh;
        }

        internal void ResetMesh(int numVertsPerCircle, int numVertsAlongRope, bool useFlatShading)
        {
            this._useFlatShading = useFlatShading;

            _vertices = new Vector3[numVertsPerCircle * numVertsAlongRope];
            _uvs = new Vector2[_vertices.Length];

            int numMainTriangles = (numVertsPerCircle * (numVertsAlongRope - 1)) * 2;
            _triangles = new int[numMainTriangles * 3];
            _triangleIndex = 0;
        }
    }
}