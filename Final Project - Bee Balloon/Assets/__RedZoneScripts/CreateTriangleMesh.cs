using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class CreateTriangleMesh : MonoBehaviour
{
    void Start()
    {
        // Get the MeshFilter component
        MeshFilter meshFilter = GetComponent<MeshFilter>();

        // Create a new mesh
        Mesh mesh = new Mesh();

        // Define the vertices of the triangle
        Vector3[] vertices = new Vector3[3]
        {
            new Vector3(0, 0, 0),  // Bottom-left corner
            new Vector3(1, 0, 0),  // Bottom-right corner
            new Vector3(0.5f, 0.5f, 0) // Top center
        };

        // Define the triangle indices (order of vertices)
        int[] triangles = new int[]
        {
            0, 1, 2 // Use the three vertices to form one triangle
        };

        // Optional: Define UVs for texturing
        /*Vector2[] uvs = new Vector2[3]
        {
            new Vector2(0, 0), // Bottom-left corner
            new Vector2(1, 0), // Bottom-right corner
            new Vector2(0.5f, 1) // Top center
        };*/

        // Assign data to the mesh
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        //mesh.uv = uvs;

        // Recalculate normals for lighting
        mesh.RecalculateNormals();

        // Assign the mesh to the MeshFilter
        meshFilter.mesh = mesh;

        // Allow collision detection
        MeshCollider collider = GetComponent<MeshCollider>();
        collider.sharedMesh = mesh;
    }
}