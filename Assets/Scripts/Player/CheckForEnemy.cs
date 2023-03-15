using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckForEnemy : MonoBehaviour
{
    public Transform enemyTransform;
    public float sightDistance = 100f;
    public Transform originPoint;
    private float enemySpeed;

    public Animator anim;

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(originPoint.position, transform.forward, out hit, sightDistance))
        {
            if (hit.transform.gameObject.tag == "Enemy")
            {
                enemyTransform = hit.transform;
                enemySpeed = Mathf.Abs(enemyTransform.GetComponent<CheckForPlayer>().speed);
                enemyTransform.GetComponent<CheckForPlayer>().speed = -enemySpeed;
            }
        }

        else if (enemyTransform)
            enemyTransform.GetComponent<CheckForPlayer>().speed = enemySpeed;

        Vector3 dir = transform.TransformDirection(Vector3.forward) * sightDistance;

        Debug.DrawRay(originPoint.position, dir, Color.magenta);

    }
}