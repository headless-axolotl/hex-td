using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lotl.Generic.Variables;
using Lotl.Hexgrid;
using Lotl.Utility;

namespace Lotl.Rendering
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class HexGridRenderer : MonoBehaviour
    {
        [Header("Grid Data")]
        [Range(0, 16)]
        [SerializeField] private int gridRaduis;

        [Header("Configuration")]
        [SerializeField] private FloatReference gridSize;
        [SerializeField] private FloatReference outerRadius;
        [SerializeField] private FloatReference innerRadius;
        [SerializeField] private FloatReference height;
        [SerializeField] private bool modifyEdgeSize;
        [SerializeField] private Material material;

        private Mesh mesh;
        private MeshFilter meshFilter;
        private MeshRenderer meshRenderer;

        private List<Hex> hexes;
        private List<Face> faces;

        private void Awake()
        {
            meshFilter = GetComponent<MeshFilter>();
            meshRenderer = GetComponent<MeshRenderer>();

            mesh = new() { name = "Hex" };
            meshFilter.mesh = mesh;
            meshRenderer.material = material;
        }

        private void OnEnable()
        {
            DrawMesh();
        }

        private void DrawMesh()
        {
            GenerateHexes();
            DrawFaces();
            CombineFaces();
        }

        private void GenerateHexes()
        {
            hexes = Hex.Spiral(Hex.Zero, gridRaduis);
        }

        private void DrawFaces()
        {
            faces = new();

            int innerHexesCount = Hex.SubhexagonCount(gridRaduis - 1);
            if (gridRaduis == 0) innerHexesCount = 0;

            for (int i = 0; i < innerHexesCount; i++)
            {
                Vector3 offset = Hex.HexToPixel(hexes[i], gridSize).xz();
                bool drawOuterFaces = outerRadius < gridSize;
                DrawHexFaces(offset, outerRadius, drawOuterFaces);
            }

            for (int i = innerHexesCount; i < hexes.Count; i++)
            {
                Vector3 offset = Hex.HexToPixel(hexes[i], gridSize).xz();
                float modifiedOuterRadius = modifyEdgeSize ? 2 * outerRadius - innerRadius : outerRadius;
                DrawHexFaces(offset, modifiedOuterRadius, drawOuterFaces: true);
            }
        }

        private void DrawHexFaces(Vector3 offset, float outerRadius = 1f, bool drawOuterFaces = false)
        {
            // Top faces:
            for (int point = 0; point < 6; point++)
            {
                Face face = CreateFace(
                    offset, outerRadius, innerRadius,
                    height / 2f, height / 2f, point);
                
                faces.Add(face);
            }

            if (height == 0f) return;

            // Bottom faces:
            for (int point = 0; point < 6; point++)
            {
                Face face = CreateFace(
                    offset, outerRadius, innerRadius,
                    -height / 2f, -height / 2f, point, true);
                faces.Add(face);
            }

            // Outer faces:
            if (drawOuterFaces)
            for (int point = 0; point < 6; point++)
            {
                Face face = CreateFace(
                    offset, outerRadius, outerRadius,
                    height / 2f, -height / 2f, point, true);
                faces.Add(face);
            }

            if (innerRadius == 0f) return;

            // Inner faces:
            for (int point = 0; point < 6; point++)
            {
                Face face = CreateFace(
                    offset, innerRadius, innerRadius,
                    height / 2f, -height / 2f, point);
                faces.Add(face);
            }
        }

        private Face CreateFace(
            Vector3 offset,
            float outerRadius, float innerRadius,
            float heightA, float heightB,
            int point, bool reverse = false)
        {

            int alternativePoint = (point < 5 ? point + 1 : 0);

            Vector3 pointA = offset + GetPoint(outerRadius, heightA, point);
            Vector3 pointB = offset + GetPoint(outerRadius, heightA, alternativePoint);
            Vector3 pointC = offset + GetPoint(innerRadius, heightB, alternativePoint);
            Vector3 pointD = offset + GetPoint(innerRadius, heightB, point);

            List<Vector3> vertices = new() { pointA, pointB, pointC, pointD };
            List<int> triangles = new() { 0, 1, 2, 2, 3, 0 }; // the indeces of the vertices
            List<Vector2> uvs = new() { Vector2.zero, Vector2.right, Vector2.one, Vector2.up };

            if (reverse)
            {
                vertices.Reverse();
            }

            return new Face(vertices, triangles, uvs);
        }

        protected Vector3 GetPoint(float size, float height, int index)
        {
            float degrees = 60f * index;
            float radians = degrees * Mathf.Deg2Rad;
            float x = Mathf.Sin(radians), z = Mathf.Cos(radians);
            return new Vector3(size * x, height, size * z);
        }

        private void CombineFaces()
        {
            List<Vector3> vertices = new();
            List<int> triangles = new();
            List<Vector2> uvs = new();

            faces.Reverse();

            for (int i = 0; i < faces.Count; i++)
            {
                vertices.AddRange(faces[i].Vertices);
                uvs.AddRange(faces[i].UVs);
                int offset = i << 2;
                foreach (int triangle in faces[i].Triangles)
                {
                    triangles.Add(triangle + offset);
                }
            }

            mesh.Clear();
            mesh.vertices = vertices.ToArray();
            mesh.triangles = triangles.ToArray();
            mesh.uv = uvs.ToArray();
            mesh.RecalculateNormals();
        }

#if UNITY_EDITOR

        private void OnValidate()
        {
            if (Application.isPlaying && mesh != null)
            {
                DrawMesh();
            }
        }

#endif
    }
}