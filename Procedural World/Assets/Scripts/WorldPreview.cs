using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldPreview
{
    Mesh mesh;
    int chunkResolution;
    float chunkSize;
    public Vector2 origin;
    Vector2[,] origins;

    //Vector2 chunkOrigin = new Vector2();
    Vector3[] vertices;
    public WorldPreview(Mesh mesh, int worldSize, float chunkScale, int chunkResolution, Vector2 origin, Vector2[,] origins)
    {
        this.chunkResolution = chunkResolution;
        this.mesh = mesh;
        this.origin = origin;
        this.origins = origins;

        int chunks = Mathf.RoundToInt(worldSize * chunkScale);
        if (chunks == 0)
        {
            chunks = 1;
            this.chunkSize = (float)worldSize / (float)chunks;
        }
        else
        {
            this.chunkSize = (float)worldSize / (float)chunks;
        }
    }
    public void generateChunk()
    {
        float chunkSpread = WorldData.chunkSize / chunkResolution;

        vertices = new Vector3[(chunkResolution + 1) * (chunkResolution + 1)];

        int count = 0;
        for (int i = 0; i < chunkResolution + 1; i++)
        {
            for (int j = 0; j < chunkResolution + 1; j++)
            {
                float x, y, z;
                x = j * chunkSpread + origins[(int)origin.x, (int)origin.y].x;
                z = origins[(int)origin.x, (int)origin.y].y - i * chunkSpread;
                y = Mathf.PerlinNoise(x * 0.05f, z * 0.05f) * 20.0f;

                vertices[count] = new Vector3(x, y, z);
                count++;
            }
        }

        int[] tris = new int[6 * (chunkResolution) * (chunkResolution)];

        count = 0;
        for (int i = 0; i < chunkResolution; i++)
        {
            for (int j = 0; j < chunkResolution; j++)
            {
                tris[count] = i * (chunkResolution + 1) + (chunkResolution + 1) + j;
                tris[count + 1] = i * (chunkResolution + 1) + j;
                tris[count + 2] = i * (chunkResolution + 1) + (chunkResolution + 1) + j + 1;

                tris[count + 3] = i * (chunkResolution + 1) + j + 1;
                tris[count + 4] = i * (chunkResolution + 1) + (chunkResolution + 1) + j + 1;
                tris[count + 5] = i * (chunkResolution + 1) + j;

                count += 6;
            }
        }
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = tris;
        mesh.RecalculateNormals();
    }
}
