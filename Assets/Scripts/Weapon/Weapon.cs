using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Rigidbody projectilePrefab;
    public Transform projectileSpawnPoint;
    public float projectileForce = 30;

    // Start is called before the first frame update
    void Start()
    {

    }



    public void Fire()
    {
        if (projectilePrefab && projectileSpawnPoint)
        {
            Rigidbody temp = Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);

            temp.AddForce(projectileSpawnPoint.forward * projectileForce, ForceMode.Impulse);

            Destroy(temp.gameObject, 2.0f);
        }
    }
}
