using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkChecker : MonoBehaviour
{
    public Vector2 chunkOrigin = new Vector2(0,0);
    Vector2 vec2 = new Vector2(0,1);
    Vector2 vec3 = new Vector2(1,1);
    public CreateWorld createWorld;

    void Start()
    {
        LoadChunk loadChunk = new LoadChunk(createWorld.worldSize, createWorld.chunkScale, createWorld.chunkResolution, chunkOrigin, createWorld.origins);
        LoadChunk loadChunk2 = new LoadChunk(createWorld.worldSize, createWorld.chunkScale, createWorld.chunkResolution, vec2, createWorld.origins);
        LoadChunk loadChunk3 = new LoadChunk(createWorld.worldSize, createWorld.chunkScale, createWorld.chunkResolution, vec3, createWorld.origins);
    }
}
