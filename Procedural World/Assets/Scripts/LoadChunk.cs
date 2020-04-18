using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadChunk
{
    Mesh mesh = new Mesh();
    int chunkResolution;
    float chunkSize;

    //Vector2 chunkOrigin = new Vector2();
    Vector3[] vertices;
    public LoadChunk(int worldSize, float chunkScale, int chunkResolution, Vector2 origin, Vector2[,] origins)
    {
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

        this.chunkResolution = chunkResolution;

        generateChunk(origin, origins);
    }
    private GameObject generateChunk(Vector2 origin, Vector2[,] origins)
    {
        float chunkSpread = chunkSize / chunkResolution;

        vertices = new Vector3[(chunkResolution+1) * (chunkResolution+1)];

        int count = 0;
        for(int i = 0; i < chunkResolution+1; i++)
        {
            for (int j = 0; j < chunkResolution+1; j++)
            {
                float x, y, z;
                x = j * chunkSpread + origins[(int)origin.x, (int)origin.y].x;
                y = 0;
                z = origins[(int)origin.x, (int)origin.y].y - i* chunkSpread;

                vertices[count] = new Vector3(x, y, z);
                count++;
            }
        }

        int[] tris = new int[6 * (chunkResolution) * (chunkResolution)];

        count = 0;
        for(int i = 0; i < chunkResolution; i++)
        {
            for (int j = 0; j < chunkResolution; j++)
            {
                tris[count] = i * (chunkResolution+1) + (chunkResolution + 1) + j;
                tris[count + 1] = i * (chunkResolution + 1) + j;
                tris[count + 2] = i * (chunkResolution + 1) + (chunkResolution + 1) + j + 1;

                tris[count + 3] = i * (chunkResolution + 1) + j + 1;
                tris[count + 4] = i * (chunkResolution + 1) + (chunkResolution + 1) + j + 1;
                tris[count + 5] = i * (chunkResolution + 1) + j;

                count += 6;
            }
        }
        mesh.vertices = vertices;
        mesh.triangles = tris;
        mesh.RecalculateNormals();

        GameObject chunk = new GameObject();
        chunk.transform.parent = GameObject.Find("World").transform;
        chunk.AddComponent<MeshFilter>().mesh = mesh;
        chunk.AddComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("Standard"));
        return chunk;
    }
}
