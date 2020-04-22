using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class World : MonoBehaviour
{
    [Range(1, 1000)]
    public float chunkSize = 10;

    [Range(2, 256)]
    public int chunkResolution = 10;

    [Range(0, 10)]
    public int chunkRenderDistance = 1;

    public List<Noise> noiseLayers;

    ChunkRenderer chunkRenderer;
    void OnValidate()
    {
        EditorApplication.delayCall += () =>
        {
            GameObject[] destroyChunks = GameObject.FindGameObjectsWithTag("Chunk");
            foreach(GameObject destroyChunk in destroyChunks)
            {
                DestroyImmediate(destroyChunk);
            }

            Chunk newChunk = new Chunk(chunkSize, chunkResolution, new Vector2(0, 0));
            chunkRenderer = new ChunkRenderer(newChunk, chunkRenderDistance);
            chunkRenderer.RenderChunks();
        };
    }
}
