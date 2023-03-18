using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attach : MonoBehaviour
{
    Transform attachPoint;
    GameObject currentWeapon;

    // Start is called before the first frame update
    void Start()
    {
        if (!attachPoint)
            attachPoint = GameObject.FindGameObjectWithTag("Attach").transform;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (currentWeapon)
            {
                attachPoint.DetachChildren();
                currentWeapon.GetComponent<Rigidbody>().isKinematic = false;
                currentWeapon.GetComponent<Rigidbody>().AddForce(transform.forward * 30, ForceMode.Impulse);
                Physics.IgnoreCollision(currentWeapon.GetComponent<Collider>(), GetComponent<Collider>(), false);
                currentWeapon = null;

            }
        }

        if (Input.GetButtonDown("Fire1") && currentWeapon)
            currentWeapon.GetComponent<Weapon>().Fire();
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.tag == "Weapon")
        {
            currentWeapon = hit.gameObject;
            currentWeapon.GetComponent<Rigidbody>().isKinematic = true;
            currentWeapon.transform.SetPositionAndRotation(attachPoint.position, attachPoint.rotation);
            currentWeapon.transform.parent = attachPoint;
            Physics.IgnoreCollision(transform.GetComponent<Collider>(), currentWeapon.GetComponent<Collider>());
        }
    }
}