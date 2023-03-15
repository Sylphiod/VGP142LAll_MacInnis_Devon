using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Animator anim;
    public Animator eAnim;

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Enemy"))
        {
            eAnim.SetTrigger("Death");
            Destroy(other.gameObject, 5f);
            Destroy(gameObject);
        }
    }
}