using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kick : MonoBehaviour
{
    public Animator anim;
    public Animator eAnim;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            eAnim.SetTrigger("Death");
            Destroy(other.gameObject, 5f);
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Punch") || anim.GetCurrentAnimatorStateInfo(0).IsName("Kick"));

        }
    }
}