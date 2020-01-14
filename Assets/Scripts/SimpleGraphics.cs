using UnityEngine;
using Unity.Mathematics;

public static class SimpleGraphics
{
    public static Mesh CreateMesh(float x, float y)
    {
        Vector3[] vertices = new Vector3[4];
        Vector2[] uv = new Vector2[4];
        int[] triangles = new int[6];

        vertices[0] = new float3(-1 * x / 2,-1 * y / 2, 0); //bottomleft
        vertices[1] = new float3( 1 * x / 2,-1 * y / 2, 0); //bottomright
        vertices[2] = new float3(-1 * x / 2, 1 * y / 2, 0); //topleft
        vertices[3] = new float3( 1 * x / 2, 1 * y / 2, 0); //topright

        uv[0] = new float2(0, 0);
        uv[1] = new float2(1, 0);
        uv[2] = new float2(0, 1);
        uv[3] = new float2(1, 1);

        triangles[0] = 0;
        triangles[1] = 3;
        triangles[2] = 1;

        triangles[3] = 3;
        triangles[4] = 0;
        triangles[5] = 2;

        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;

        return mesh;
    }
}
