using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk
{
    public GameObject chunkGameObject;
    public Vector2 origin;
    public float chunkSize;
    public int chunkResolution;

    public Chunk(float chunkSize, int chunkResolution, Vector2 originPosition)
    {
        this.chunkSize = chunkSize;
        this.chunkResolution = chunkResolution;
        this.origin = DetermineOrigin(originPosition);

        generateChunk();
    }

    private void generateChunk()
    {
        Mesh mesh = new Mesh();
        Vector3[] vertices = new Vector3[chunkResolution * chunkResolution];
        int[] tris = new int[6 * (chunkResolution - 1) * (chunkResolution - 1)];

        int count = 0;
        for (int i = 0; i < chunkResolution; i++)
        {
            for (int j = 0; j < chunkResolution; j++)
            {
                float x, y, z;
                x = j * chunkSize / (chunkResolution - 1);
                z = i * chunkSize / (chunkResolution - 1);
                y = Mathf.PerlinNoise(x * 0.05f, z * 0.05f) * 20.0f;

                vertices[count] = new Vector3(x, y, z);
                count++;
            }
        }

        count = 0;
        for (int i = 0; i < chunkResolution - 1; i++)
        {
            for (int j = 0; j < chunkResolution - 1; j++)
            {
                tris[count] = i * (chunkResolution) + j;
                tris[count + 1] = i * (chunkResolution) + (chunkResolution) + j;
                tris[count + 2] = i * (chunkResolution) + j + 1;

                tris[count + 3] = i * (chunkResolution) + (chunkResolution) + j;
                tris[count + 5] = i * (chunkResolution) + j + 1;
                tris[count + 4] = i * (chunkResolution) + (chunkResolution) + j + 1;

                count += 6;
            }
        }

        mesh.vertices = vertices;
        mesh.triangles = tris;
        mesh.RecalculateNormals();

        chunkGameObject = new GameObject();
        chunkGameObject.name = "" + origin.x + "," + origin.y;
        chunkGameObject.tag = "Chunk";
        chunkGameObject.transform.parent = GameObject.Find("World").transform;
        chunkGameObject.transform.position = new Vector3(origin.x, 0, origin.y);
        chunkGameObject.AddComponent<MeshFilter>().mesh = mesh;
        chunkGameObject.AddComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("Standard"));
        chunkGameObject.AddComponent<MeshCollider>();
        chunkGameObject.AddComponent<BoxCollider>();
        BoxCollider col = chunkGameObject.GetComponent<BoxCollider>();
        col.size = new Vector3(col.size.x, 100, col.size.z);
        col.isTrigger = true;
    }

    private Vector2 DetermineOrigin(Vector2 position)
    {
        float x, z;
        if (position.x < 0)
        {
            x = Mathf.Ceil(position.x / chunkSize) * chunkSize;
        }
        else
        {
            x = Mathf.Floor(position.x / chunkSize) * chunkSize;
        }

        if(position.y < 0)
        {
            z = Mathf.Ceil(position.y / chunkSize) * chunkSize;
        }
        else
        {
            z = Mathf.Floor(position.y / chunkSize) * chunkSize;
        }
        return new Vector2(x, z);
    }
}
