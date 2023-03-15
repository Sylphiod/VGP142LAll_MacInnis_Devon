using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckForPlayer : MonoBehaviour
{
    public Transform playerTransform;
    public float sightDistance = 100f;
    public Transform originPoint;
    public float speed;
    public float rotationSpeed;
    private Vector3 playerPosition;
    public Animator anim;

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (!playerTransform)
            transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
        else if (speed < 0)
            transform.rotation = playerTransform.rotation;
        else
            transform.LookAt(playerPosition);


        RaycastHit hit;
        if (Physics.Raycast(originPoint.position, transform.forward, out hit, sightDistance))
        {


            if (hit.transform.gameObject.tag == "Player")
            {
                playerTransform = hit.transform;

            }
        }

        Vector3 dir = transform.TransformDirection(Vector3.forward) * sightDistance;

        Debug.DrawRay(originPoint.position, dir, Color.red);

        if (playerTransform)
        {
            playerPosition = new Vector3(playerTransform.position.x, 7.77f, playerTransform.position.z);

            transform.position = Vector3.MoveTowards(transform.position, playerPosition, speed);
        }


    }
}
