using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkRenderer
{
    Vector2 currentChunkCoordinates;
    Chunk currentChunk;

    List<Vector2> chunkNeighbors = new List<Vector2>();
    List<Chunk> renderedChunks = new List<Chunk>();

    int chunkResolution;
    int chunkRenderDistance;
    float chunkSize;

    public ChunkRenderer(Chunk currentChunk, int chunkRenderDistance)
    {
        this.currentChunkCoordinates = currentChunk.origin;
        this.chunkSize = currentChunk.chunkSize;
        this.chunkRenderDistance = chunkRenderDistance;
        this.chunkResolution = currentChunk.chunkResolution;
        this.currentChunk = currentChunk;

        if (!renderedChunks.Contains(currentChunk))
        {
            renderedChunks.Add(currentChunk);
        }
    }

    public void RenderChunks()
    {
        getChunkNeighbors();
        deleteChunks();
        addChunks();
    }

    private void addChunks()
    {
        List<Vector2> origins = new List<Vector2>();
        foreach(Chunk chunk in renderedChunks)
        {
            origins.Add(chunk.origin);
        }
        foreach(Vector2 neighbor in chunkNeighbors)
        {
            if (!origins.Contains(neighbor) && neighbor != currentChunk.origin)
            {
                renderedChunks.Add(new Chunk(chunkSize, chunkResolution, neighbor));
            }
        }
    }

    private void getChunkNeighbors()
    {
        {
            for (int i = 0; i < (2 * chunkRenderDistance + 1); i++)
            {
                for (int j = 0; j < (2 * chunkRenderDistance + 1); j++)
                {
                    float x = currentChunkCoordinates.x - chunkRenderDistance * chunkSize + j * chunkSize;
                    float y = currentChunkCoordinates.y - chunkRenderDistance * chunkSize + i * chunkSize;

                    chunkNeighbors.Add(new Vector2(x, y));
                }
            }
        }
    }

    private void deleteChunks()
    {
        foreach(Chunk chunk in renderedChunks)
        {
            if (!chunkNeighbors.Contains(chunk.origin))
            {
                GameObject.Destroy(chunk.chunkGameObject);
                renderedChunks.Remove(chunk);
            }
        }
    }
}
