using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;
    public float distanceThreshold = 10f;

    private void LateUpdate()
    {
        Vector3 desiredPosition = target.position + offset;
        Vector3 difference = desiredPosition - transform.position;

        if (difference.magnitude > distanceThreshold)
        {
            desiredPosition = transform.position + difference.normalized * distanceThreshold;
        }

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}