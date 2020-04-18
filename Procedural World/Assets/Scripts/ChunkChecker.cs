using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkChecker : MonoBehaviour
{
    public Vector2 chunkOrigin = new Vector2(0,0);
    public CreateWorld createWorld = new CreateWorld();
    private void OnValidate()
    {
        LoadChunk loadChunk = new LoadChunk();
    }
}
