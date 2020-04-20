using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnLocation : MonoBehaviour
{
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(player, new Vector3(WorldData.worldSize / 2 + WorldData.chunkSize / 2, 20, WorldData.worldSize / 2 - WorldData.chunkSize / 2), Quaternion.identity);
    }
}
