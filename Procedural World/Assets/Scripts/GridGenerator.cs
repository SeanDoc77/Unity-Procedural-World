using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator
{
    public Vector2[,] chunkOrigins;

    int worldSize;
    int chunkScale;

    public GridGenerator(int worldSize, int chunkScale)
    {
        this.worldSize = worldSize;
        this.chunkScale = chunkScale;

        generateChunkGrid();
    }

    private void generateChunkGrid()
    {
        int chunks = Mathf.RoundToInt(worldSize * (chunkScale/100.0f));
        Debug.Log(chunks);
        float chunkSize;

        chunkOrigins = new Vector2[chunks,chunks];

        if (chunks == 0)
        {
            chunkSize = 1;
        }
        else
        {
            chunkSize = worldSize / chunks;
        }

        float xStart = -worldSize / 2 + chunkSize/2;
        float zStart = worldSize / 2 - chunkSize / 2; ;

        int count = 0;

        for (int i = 0; i < chunks; i++)
        {
            for (int j = 0; j < chunks; j++)
            {
                float x, z;
                x = xStart + j * chunkSize;
                z = zStart - i * chunkSize;
                Vector2 vertex = new Vector2(x, z);
                chunkOrigins[i,j] = vertex;
                count++;
            }
        }
    }
}
