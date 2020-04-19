using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkChecker : MonoBehaviour
{
    [Range (1,20)]
    public int chunkRenderDistance;

    Vector2 vec2 = new Vector2(0,1);
    Vector2 vec3 = new Vector2(1,1);
    public CreateWorld createWorld;

    void Start()
    {
        int chunks = Mathf.RoundToInt(createWorld.worldSize * createWorld.chunkScale);
        Vector2 spawnOrigin = new Vector2(chunks / 2, chunks / 2);

        LoadChunk loadChunk = new LoadChunk(createWorld.worldSize, createWorld.chunkScale, createWorld.chunkResolution, spawnOrigin, createWorld.origins);
    }

    private void OnTriggerEnter(Collider other)
    {
        Vector2 chunkCoordinates;
        chunkCoordinates = chunkNameToVector2(other.name);
        Debug.Log("Current Coords: " + chunkCoordinates);

        generateChunkNeighbors(chunkCoordinates);
    }

    private void generateChunkNeighbors(Vector2 chunkCoordinates)
    {
        Vector2[] chunkNeighbors = getChunkNeighbors(chunkCoordinates);

        List<string> renderedChunks = getRenderedChunks();

        for (int i = 0; i < chunkNeighbors.Length; i++)
        {
            string checkString = chunkNeighbors[i].x + "," + chunkNeighbors[i].y;
            if (renderedChunks.Contains(checkString))
            {
                Debug.Log("Chunk already exists");
            }
            else
            {
                if (withinBounds(chunkNeighbors[i]))
                {
                    LoadChunk loadChunk = new LoadChunk(createWorld.worldSize, createWorld.chunkScale, createWorld.chunkResolution, chunkNeighbors[i], createWorld.origins);
                }
                else
                {
                    Debug.Log("Coordinates not within bounds");
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

                if(x == (int)currentChunkCoordinates.x && y == (int)currentChunkCoordinates.y)
                {
                    Debug.Log("Current Chunk");
                }
                else
                {
                    chunkNeighbors[count] = new Vector2(x, y);
                    Debug.Log(chunkNeighbors[count]);
                    count++;
                }
            }
        }
        return chunkNeighbors;
    }

    private bool withinBounds(Vector2 chunkCoordinates)
    {
        int chunks = Mathf.RoundToInt(createWorld.worldSize * createWorld.chunkScale);

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
