using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBounce : MonoBehaviour
{
    public int maxBounces = 3;
    public float bounceForce = 10f;

    private int bounceCount = 0;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            if (++bounceCount >= maxBounces)
            {
                Destroy(gameObject);
            }
            else
            {
                Rigidbody rigidbody = GetComponent<Rigidbody>();
                rigidbody.AddForce(Vector3.up * bounceForce, ForceMode.Impulse);
            }
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }
}
