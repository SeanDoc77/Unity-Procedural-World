using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateWorld : MonoBehaviour
{
    [Range(1, 100)]
    public int worldSize = 10;

    [Range(1, 100)]
    public int chunkScale = 10;

    [Range(1, 100)]
    public int chunkResolution = 10;

    Vector2[,] plotOrigins;

    // Start is called before the first frame update
    private void OnValidate()
    {
        buildGrid();
    }

    private void buildGrid()
    {
        GridGenerator gridGenerator = new GridGenerator(worldSize, chunkScale);
        plotOrigins = gridGenerator.chunkOrigins;
    }
    void OnDrawGizmos()
    {
        for (int i = 0; i < plotOrigins.GetLength(0); i++)
        {
            for (int j = 0; j < plotOrigins.GetLength(1); j++)
            {
                float x, y, z;
                x = plotOrigins[i,j].x;
                z = plotOrigins[i,j].y;
                y = 0;

                Vector3 origin = new Vector3(x, y, z);
                Gizmos.DrawSphere(origin, 0.1f);
            }
        }
    }
}
