using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkChecker : MonoBehaviour
{
    [Range (1,20)]
    public int chunkRenderDistance;

    [Range(0, 1)]
    public float renderSpeedScale = 1;

    Vector2 vec2 = new Vector2(0,1);
    Vector2 vec3 = new Vector2(1,1);

    void Start()
    {
        int chunks = Mathf.RoundToInt(WorldData.worldSize * WorldData.chunkScale);
        Vector2 spawnOrigin = new Vector2(chunks / 2, chunks / 2);


        LoadChunk loadChunk = new LoadChunk(WorldData.worldSize, WorldData.chunkScale, WorldData.chunkResolution, spawnOrigin, WorldData.origins);
    }

    private void OnTriggerEnter(Collider other)
    {
        Vector2 chunkCoordinates;
        chunkCoordinates = chunkNameToVector2(other.name);

        generateChunkNeighbors(chunkCoordinates);
    }

    private void generateChunkNeighbors(Vector2 chunkCoordinates)
    {
        Vector2[] chunkNeighbors = getChunkNeighbors(chunkCoordinates);

        List<string> renderedChunks = getRenderedChunks();

        StartCoroutine(GenerateNeighboringChunks(chunkNeighbors, renderedChunks));

        string currentChunk = chunkCoordinates.x + "," + chunkCoordinates.y;
        foreach (string chunk in renderedChunks)
        {
            if(chunk != currentChunk)
            {
                if (!checkChunkNeighbors(chunk, chunkNeighbors))
                {
                    Debug.Log("Destroy");
                    Destroy(GameObject.Find(chunk));
                }
            }
        }
    }

    private List<string> getRenderedChunks()
    {
        List<string> renderedChunks = new List<string>();

        GameObject[] chunks = GameObject.FindGameObjectsWithTag("Chunk");

        for(int i = 0; i < chunks.Length; i++)
        {
            renderedChunks.Add(chunks[i].name);
        }
        return renderedChunks;
    }

    private Vector2 chunkNameToVector2(string chunkName)
    {
        string[] strCoordinates = chunkName.Split(',');
        return new Vector2(float.Parse(strCoordinates[0]), float.Parse(strCoordinates[1]));
    }

    private Vector2[] getChunkNeighbors(Vector2 currentChunkCoordinates)
    {
        Vector2[] chunkNeighbors = new Vector2[(2*chunkRenderDistance + 1) * (2 * chunkRenderDistance + 1)-1] ;

        int count = 0;
        for (int i = 0; i < (2 * chunkRenderDistance + 1); i++)
        {
            for (int j = 0; j < (2 * chunkRenderDistance + 1); j++)
            {
                int x = (int)currentChunkCoordinates.x - chunkRenderDistance + j;
                int y = (int)currentChunkCoordinates.y + chunkRenderDistance - i;

                if(!(x == (int)currentChunkCoordinates.x && y == (int)currentChunkCoordinates.y))
                {
                    chunkNeighbors[count] = new Vector2(x, y);
                    count++;
                }
            }
        }
        return chunkNeighbors;
    }

    private bool withinBounds(Vector2 chunkCoordinates)
    {
        int chunks = Mathf.RoundToInt(WorldData.worldSize * WorldData.chunkScale);

        if (chunkCoordinates.x > chunks - 1 || chunkCoordinates.y > chunks - 1 || chunkCoordinates.x < 0 || chunkCoordinates.y < 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private bool checkChunkNeighbors(string chunk, Vector2[] chunkNeighbors)
    {
        for(int i = 0; i < chunkNeighbors.Length; i++)
        {
            string checkString = chunkNeighbors[i].x + "," + chunkNeighbors[i].y;
            if (checkString == chunk)
            {
                return true;
            }
        }
        return false;
    }

    IEnumerator GenerateNeighboringChunks(Vector2[] chunkNeighbors, List<string> renderedChunks)
    {
        //Generate the neighboring chunks
        for (int i = 0; i < chunkNeighbors.Length; i++)
        {
            string checkString = chunkNeighbors[i].x + "," + chunkNeighbors[i].y;
            if (!renderedChunks.Contains(checkString))
            {
                if (withinBounds(chunkNeighbors[i]))
                {
                    yield return new WaitForSeconds(1.0f - renderSpeedScale);
                    LoadChunk loadChunk = new LoadChunk(WorldData.worldSize, WorldData.chunkScale, WorldData.chunkResolution, chunkNeighbors[i], WorldData.origins);
                }
                else
                {
                    Debug.Log("Coordinates not within bounds: " + chunkNeighbors[i]);
                }
            }
        }
    }
}
