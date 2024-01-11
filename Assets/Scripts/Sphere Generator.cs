using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;
using TMPro;
using System.Security.Cryptography;

public class SphereGenerator : MonoBehaviour
{
    public float radius = 5f;
    public Material sphereMaterial;
    public Material outlineMaterial;
    public int subdivisions = 2;
    public Color outlineColor = Color.white;
    public float outlineWidth = 0.1f;
    public int totalCenters;
    public int numberOfHexagons;
    public int result;
    public GameObject tilePrefab;
    public float rotation;
    public int verticesCount;
    List<Vector3> closestObjects;
    int index;
    Vector3[] pointArray;
    // List to store tile positions
    private List<Vector3> tilePositions = new List<Vector3>();

    // List to store outlined vertices
    private List<Vector3> outlinedVertices = new List<Vector3>();

    private Dictionary<int, int> midpointCache = new Dictionary<int, int>();


    void Start()
    {
        GenerateConwaySphere();

    }

    void GenerateConwaySphere()
    {
        GameObject sphereObject = new GameObject("ConwaySphere");
        sphereObject.transform.parent = transform;

        MeshFilter meshFilter = sphereObject.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = sphereObject.AddComponent<MeshRenderer>();
        meshRenderer.material = sphereMaterial;

        Mesh mesh = GenerateIcosphere(subdivisions);
        meshFilter.mesh = mesh;

        OutlinePentagonHexagonEdges(mesh);
    }

    void Subdivide(List<Vector3> vertices, ref List<int> triangles)
    {
        int originalVertexCount = vertices.Count;
        List<int> newTriangles = new List<int>(triangles);

        int triangleCount = newTriangles.Count / 3;

        for (int i = 0; i < triangleCount; i++)
        {
            int v1 = newTriangles[i * 3];
            int v2 = newTriangles[i * 3 + 1];
            int v3 = newTriangles[i * 3 + 2];

            int v12 = GetMidpoint(vertices, v1, v2);
            int v23 = GetMidpoint(vertices, v2, v3);
            int v31 = GetMidpoint(vertices, v3, v1);

            newTriangles.AddRange(new int[] { v1, v12, v31, v2, v23, v12, v3, v31, v23, v12, v23, v31 });
        }
        triangles.Clear();
        triangles.AddRange(newTriangles);
    }

    Mesh GenerateIcosphere(int subdivisions)
    {
        Mesh mesh = new Mesh();
        float t = (1f + Mathf.Sqrt(5f)) / 2f;

        List<Vector3> vertices = new List<Vector3>
        {
            new Vector3(-1, t, 0),
            new Vector3(1, t, 0),
            new Vector3(-1, -t, 0),
            new Vector3(1, -t, 0),

            new Vector3(0, -1, t),
            new Vector3(0, 1, t),
            new Vector3(0, -1, -t),
            new Vector3(0, 1, -t),

            new Vector3(t, 0, -1),
            new Vector3(t, 0, 1),
            new Vector3(-t, 0, -1),
            new Vector3(-t, 0, 1)
        };
        for (int i = 0; i < vertices.Count; i++)
        {
            vertices[i] = vertices[i].normalized * radius;
        }
        List<int> triangles = new List<int>
        {
            0, 11, 5,
            0, 5, 1,
            0, 1, 7,
            0, 7, 10,
            0, 10, 11,

            1, 5, 9,
            5, 11, 4,
            11, 10, 2,
            10, 7, 6,
            7, 1, 8,

            3, 9, 4,
            3, 4, 2,
            3, 2, 6,
            3, 6, 8,
            3, 8, 9,

            4, 9, 5,
            2, 4, 11,
            6, 2, 10,
            8, 6, 7,
            9, 8, 1
        };
        for (int i = 0; i < subdivisions; i++)
        {
            Subdivide(vertices, ref triangles);
        }

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();

        return mesh;
    }

    int GetMidpoint(List<Vector3> vertices, int v1, int v2)
    {
        int smallerIndex = Mathf.Min(v1, v2);
        int largerIndex = Mathf.Max(v1, v2);
        int key = (smallerIndex << 16) + largerIndex;

        if (!midpointCache.ContainsKey(key))
        {
            Vector3 midpoint = (vertices[v1] + vertices[v2]).normalized * radius;
            vertices.Add(midpoint);
            midpointCache.Add(key, vertices.Count - 1);
            if (totalCenters < numberOfHexagons)
            {
                CreateEmptyObject(midpoint, "Node Position" + (totalCenters + 1));

                totalCenters++;
            }
            return vertices.Count - 1;
        }

        return midpointCache[key];
    }
    void CreateEmptyObject(Vector3 position, string name)
    {
        GameObject emptyObject = Instantiate(tilePrefab);
        emptyObject.transform.parent = transform;
        emptyObject.transform.position = position;
        emptyObject.name = "Tile" + totalCenters;
        Vector3 normal = position.normalized;
        emptyObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, normal);
        rotation = (30 / radius) * (radius - emptyObject.transform.position.y);
        tilePositions.Add(position);
        RotateTileAroundNormal(emptyObject, new Vector3(0, rotation, 0));
    }

    public void RotateTileAroundNormal(GameObject tile, Vector3 rotation)
    {
        // Ensure the tile is a child of the sphere
        if (tile.transform.parent == transform)
        {
            Vector3 normal = tile.transform.position.normalized;

            // Rotate the tile around its normal axis
            tile.transform.rotation = Quaternion.FromToRotation(Vector3.up, normal) * Quaternion.Euler(rotation);
        }
        else
        {
            Debug.LogWarning("The provided GameObject is not a child of the sphere.");
        }
    }


    void OutlinePentagonHexagonEdges(Mesh mesh)
    {
        List<Vector3> vertices = new List<Vector3>(mesh.vertices);
        List<int> triangles = new List<int>(mesh.triangles);

        int triangleCount = triangles.Count / 3;

        for (int i = 0; i < triangleCount; i++)
        {
            int v1 = triangles[i * 3];
            int v2 = triangles[i * 3 + 1];
            int v3 = triangles[i * 3 + 2];

            int center = GetMidpoint(vertices, v1, v2, v3);

            int edgeKey1 = (Mathf.Min(v1, v2) << 16) + Mathf.Max(v1, v2);
            int edgeKey2 = (Mathf.Min(v2, v3) << 16) + Mathf.Max(v2, v3);
            int edgeKey3 = (Mathf.Min(v3, v1) << 16) + Mathf.Max(v3, v1);

            // Check if the edge belongs to a pentagon or hexagon
            if (midpointCache.ContainsKey(edgeKey1) && midpointCache.ContainsKey(edgeKey2) && midpointCache.ContainsKey(edgeKey3))
            {
                OutlineEdge(vertices[midpointCache[edgeKey1]], vertices[center]);
                OutlineEdge(vertices[midpointCache[edgeKey2]], vertices[center]);
                OutlineEdge(vertices[midpointCache[edgeKey3]], vertices[center]);
            }
        }
    }

    int GetMidpoint(List<Vector3> vertices, int v1, int v2, int v3)
    {
        // Get or add the midpoint between three vertices
        List<int> indices = new List<int> { v1, v2, v3 };
        indices.Sort();


        int key = (indices[0] << 20) + (indices[1] << 10) + indices[2];

        if (!midpointCache.ContainsKey(key))
        {
            Vector3 midpoint = (vertices[v1] + vertices[v2] + vertices[v3]) / 3f;
            midpoint = midpoint.normalized * radius;
            vertices.Add(midpoint);
            midpointCache.Add(key, vertices.Count - 1);



            // Return the index of the newly added vertex
            return vertices.Count - 1;
        }

        return midpointCache[key];
    }



    void OutlineEdge(Vector3 point1, Vector3 point2)
    {
        // Calculate the midpoint of the edge
        Vector3 midpoint = (point1 + point2) / 2f;

        // Calculate the direction from the center of the sphere to the midpoint
        Vector3 centerToMidpoint = midpoint.normalized;

        // Calculate the offset to make the edges appear bigger in the up direction
        float offset = 0.3f; // Adjust this value to control the size of the offset
        Vector3 offsetVector = centerToMidpoint * offset;

        // Calculate the new positions for the edge points with the offset
        Vector3 newPoint1 = point1 + offsetVector;
        Vector3 newPoint2 = point2 + offsetVector;

        GameObject edgeObject = new GameObject("Edge");
        edgeObject.transform.parent = transform;

        LineRenderer lineRenderer = edgeObject.AddComponent<LineRenderer>();
        lineRenderer.material = outlineMaterial;
        lineRenderer.startWidth = outlineWidth;
        lineRenderer.endWidth = outlineWidth;
        lineRenderer.startColor = outlineColor;
        lineRenderer.endColor = outlineColor;

        lineRenderer.SetPosition(0, newPoint1);
        lineRenderer.SetPosition(1, newPoint2);

        if (!outlinedVertices.Contains(newPoint1))
        {
            outlinedVertices.Add(newPoint1);
            verticesCount++;
        }

        if (!outlinedVertices.Contains(newPoint2))
        {
            outlinedVertices.Add(newPoint2);
            verticesCount++;
        }
    }

}