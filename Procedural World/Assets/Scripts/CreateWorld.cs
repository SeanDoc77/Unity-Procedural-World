using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable 1234

public class CreateWorld : MonoBehaviour
{
    [Range(1, 100000)]
    public int worldSize = 10;

    [Range(0.0001f, 1.0f)]
    public float chunkScale = 10;

    [Range(1, 255)]
    public int chunkResolution = 10;

    [Range(0, 20)]
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

        UnityEditor.EditorApplication.delayCall += () =>
        {
            int chunks = Mathf.RoundToInt(worldSize * chunkScale);
            Vector2 spawnOrigin = new Vector2(chunks / 2, chunks / 2);

            if (meshFilters == null || meshFilters.Length != (2 * previewChunkRenderDistance + 1) * (2 * previewChunkRenderDistance + 1))
            {
                meshFilters = new MeshFilter[(2 * previewChunkRenderDistance + 1) * (2 * previewChunkRenderDistance + 1)];
                GameObject[] previewChunks = GameObject.FindGameObjectsWithTag("Chunk Preview");
                foreach (GameObject chunk in previewChunks)
                {
                    DestroyImmediate(chunk);
                }
            }

            previews = new WorldPreview[(2 * previewChunkRenderDistance + 1) * (2 * previewChunkRenderDistance + 1)];
            Vector2[] chunkNeighors = getChunkNeighbors(spawnOrigin);

            for (int i = 0; i < (2 * previewChunkRenderDistance + 1) * (2 * previewChunkRenderDistance + 1); i++)
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
                previews[i] = new WorldPreview(meshFilters[i].sharedMesh, worldSize, chunkScale, chunkResolution, chunkNeighors[i], origins);
            }
            generateMeshes();
        };
    }

    void generateMeshes()
    {
        bool warning = false;
        foreach(WorldPreview preview in previews)
        {
            if (withinBounds(preview.origin))
            {
                preview.generateChunk();
            }
            else
            {
                warning = true;
            }
        }

        if (warning)
        {
            Debug.LogWarning("You are attempting generate chunks outside of the grid boundaries");
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

    private Vector2[] getChunkNeighbors(Vector2 currentChunkCoordinates)
    {
        Vector2[] chunkNeighbors = new Vector2[(2 * previewChunkRenderDistance + 1) * (2 * previewChunkRenderDistance + 1)];

        int count = 0;
        for (int i = 0; i < (2 * previewChunkRenderDistance + 1); i++)
        {
            for (int j = 0; j < (2 * previewChunkRenderDistance + 1); j++)
            {
                int x = (int)currentChunkCoordinates.x - previewChunkRenderDistance + j;
                int y = (int)currentChunkCoordinates.y + previewChunkRenderDistance - i;

                chunkNeighbors[count] = new Vector2(x, y);
                count++;
                
            }
        }
        return chunkNeighbors;
    }

    private bool withinBounds(Vector2 chunkCoordinates)
    {
        int chunks = Mathf.RoundToInt(worldSize * chunkScale);

        if (chunkCoordinates.x > chunks - 1 || chunkCoordinates.y > chunks - 1 || chunkCoordinates.x < 0 || chunkCoordinates.y < 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}

