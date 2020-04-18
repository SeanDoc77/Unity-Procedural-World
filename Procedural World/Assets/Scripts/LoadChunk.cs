using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadChunk
{
    Mesh mesh = new Mesh();
    public int chunkResolution;
    public float chunkSize;

    //Vector2 chunkOrigin = new Vector2();
    Vector3[] vertices;
    public LoadChunk(int worldSize, float chunkScale, int chunkResolution, Vector2 origin, Vector2[,] origins)
    {
        int chunks = Mathf.RoundToInt(worldSize * (chunkScale / 100.0f));

        if (chunks == 0)
        {
            this.chunkSize = 1;
        }
        else
        {
            this.chunkSize = worldSize / chunks;
        }

        this.chunkResolution = chunkResolution;

        generateChunk(origin, origins);
    }
    private GameObject generateChunk(Vector2 origin, Vector2[,] origins)
    {
        float spread = chunkSize / chunkResolution;

        vertices = new Vector3[chunkResolution * chunkResolution];

        Debug.Log(origins[0,0]);

        int count = 0;
        for(int i = 0; i < chunkResolution; i++)
        {
            for (int j = 0; j < chunkResolution; j++)
            {
                float x, y, z;
                x = j * spread + origins[(int)origin.x, (int)origin.y].x;
                y = 0;
                z = i * spread + origins[(int)origin.x, (int)origin.y].x;

                vertices[count] = new Vector3(x, y, z);
                count++;
            }
        }

        int[] tris = new int[6 * (chunkResolution - 1) * (chunkResolution - 1)];

        count = 0;
        for(int i = 0; i < chunkResolution-1; i++)
        {
            for (int j = 0; j < chunkResolution-1; j++)
            {
                tris[count] = i * chunkResolution + chunkResolution + j + 1;
                tris[count + 1] = i * chunkResolution + j;
                tris[count + 2] = i * chunkResolution + chunkResolution + j;

                tris[count + 3] = i * chunkResolution + j;
                tris[count + 4] = i * chunkResolution + chunkResolution + j + 1;
                tris[count + 5] = i * chunkResolution + j + 1;

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
