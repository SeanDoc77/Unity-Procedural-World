using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateWorld : MonoBehaviour
{
    [Range(1, 100000)]
    public int worldSize = 10;

    [Range(0.0001f, 1.0f)]
    public float chunkScale = 10;

    [Range(1, 255)]
    public int chunkResolution = 10;

    [Range(1, 20)]
    public int previewChunkRenderDistance = 3;

    private Vector2[,] origins;
    WorldPreview[] previews;
    MeshFilter[] meshFilters;

    // Start is called before the first frame update
    void Awake()
    {
        WorldData.worldSize = worldSize;
        WorldData.chunkScale = chunkScale;
        WorldData.chunkResolution = chunkResolution;
        WorldData.origins = origins;
    }
    void OnValidate()
    {
        buildGrid();

        int chunks = Mathf.RoundToInt(worldSize * chunkScale);
        Vector2 spawnOrigin = new Vector2(chunks / 2, chunks / 2);

        if (meshFilters == null || meshFilters.Length != previewChunkRenderDistance * previewChunkRenderDistance)
        {
            meshFilters = new MeshFilter[previewChunkRenderDistance * previewChunkRenderDistance];
            GameObject[] previewChunks = GameObject.FindGameObjectsWithTag("Chunk Preview");
            foreach(GameObject chunk in previewChunks)
            {
                UnityEditor.EditorApplication.delayCall += () =>
                {
                    DestroyImmediate(chunk);
                };
            }
        }

        previews = new WorldPreview[previewChunkRenderDistance * previewChunkRenderDistance];

        for (int i = 0; i < previewChunkRenderDistance * previewChunkRenderDistance; i++)
        {
            if (meshFilters[i] == null)
            {
                GameObject worldPreview = new GameObject();
                worldPreview.transform.parent = gameObject.transform.GetChild(0);
                worldPreview.tag = "Chunk Preview";

                worldPreview.AddComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("Standard"));
                meshFilters[i] = worldPreview.AddComponent<MeshFilter>();
                meshFilters[i].sharedMesh = new Mesh();
            }

            previews[i] = new WorldPreview(meshFilters[i].sharedMesh, worldSize, chunkScale, chunkResolution, spawnOrigin, origins);
        }
        generateMeshes();
    }

    void generateMeshes()
    {
        foreach(WorldPreview preview in previews)
        {
            preview.generateChunk();
        }
    }
    private void buildGrid()
    {
        GridGenerator gridGenerator = new GridGenerator(worldSize, chunkScale);
        origins = gridGenerator.chunkOrigins;
    }

    void OnDrawGizmos()
    {
        int chunks = Mathf.RoundToInt(worldSize * chunkScale);
        for (int i = 0; i < origins.GetLength(0); i++)
        {
            for (int j = 0; j < origins.GetLength(1); j++)
            {
                float x, y, z;
                x = origins[i, j].x;
                z = origins[i, j].y;
                y = 0;

                Vector3 origin = new Vector3(x, y, z);
                Gizmos.DrawSphere(origin, 0.1f*worldSize/chunks);
            }
        }
    }
}

