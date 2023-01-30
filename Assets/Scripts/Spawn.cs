using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public GameObject prefabToSpawn;
    public float spawnDelay = 30f;

    private float timeSinceLastSpawn;

    private void Update()
    {
        timeSinceLastSpawn += Time.deltaTime;

        if (timeSinceLastSpawn >= spawnDelay)
        {
            Instantiate(prefabToSpawn, transform.position, transform.rotation);
            timeSinceLastSpawn = 0f;
        }
    }
}
