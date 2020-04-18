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
    public LoadChunk(float chunkSize, int chunkResolution, Vector2 origin)
    {
        this.chunkSize = chunkSize;
        this.chunkResolution = chunkResolution;

        generateChunk(origin);
    }
    private GameObject generateChunk(Vector2 orgin)
    {
        float spread = chunkSize / chunkResolution;

        vertices = new Vector3[chunkResolution * chunkResolution];

        Debug.Log(chunkResolution);

        int count = 0;
        for(int i = 0; i < chunkResolution; i++)
        {
            for (int j = 0; j < chunkResolution; j++)
            {
                float x, y, z;
                x = j * spread;
                y = 0;
                z = i * spread;

                vertices[count] = new Vector3(x, y, z);
                Debug.Log("x: " + x);
                count++;
            }
        }

        int[] tris = new int[6 * (chunkResolution - 1) * (chunkResolution - 1)];

        count = 0;
        for(int i = 0; i < chunkResolution; i++)
        {
            for (int j = 0; j < chunkResolution; j++)
            {
                tris[count] = i*chunkResolution + chunkResolution + j;
                tris[count + 1] = i*chunkResolution + j;
                tris[count + 2] = i * chunkResolution + chunkResolution + j + 1;

                tris[count + 3] = i * chunkResolution + j;
                tris[count + 4] = i * chunkResolution + j + 1;
                tris[count + 5] = i * chunkResolution + chunkResolution + j + 1;

                count++;
            }
        }
        mesh.triangles = tris;

        GameObject chunk = new GameObject();
        chunk.transform.parent = GameObject.Find("World").transform;
        chunk.AddComponent<MeshFilter>().mesh = mesh;
        chunk.AddComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("Standard"));
        return chunk;
    }
}
