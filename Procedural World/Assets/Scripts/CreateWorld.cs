using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateWorld : MonoBehaviour
{
    [Range(1, 1000)]
    public int worldSize = 10;

    [Range(0.0001f, 1.0f)]
    public float chunkScale = 10;

    [Range(1, 100)]
    public int chunkResolution = 10;

    [HideInInspector]
    public Vector2[,] origins;

    // Start is called before the first frame update
    private void OnValidate()
    {
        buildGrid();
    }

    private void buildGrid()
    {
        GridGenerator gridGenerator = new GridGenerator(worldSize, chunkScale);
        origins = gridGenerator.chunkOrigins;
    }
    void OnDrawGizmos()
    {
        for (int i = 0; i < origins.GetLength(0); i++)
        {
            for (int j = 0; j < origins.GetLength(1); j++)
            {
                float x, y, z;
                x = origins[i,j].x;
                z = origins[i,j].y;
                y = 0;

                Vector3 origin = new Vector3(x, y, z);
                Gizmos.DrawSphere(origin, 0.1f);
            }
        }
    }
}
