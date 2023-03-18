using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpawnPoint : MonoBehaviour
{
    public GameObject[] collectiblePrefabArray;

    void Start()
    {
        try
        {
            int randomIndex = UnityEngine.Random.Range(0, collectiblePrefabArray.Length);

            Instantiate(collectiblePrefabArray[randomIndex], transform.position, transform.rotation);
            throw new UnassignedReferenceException("Coin did not spawn " + name + " unable to play");
        }

        finally
        {
            Debug.Log("Coins have been positioned. ");
        }

    }
}
