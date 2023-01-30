using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
   
    public Transform player;
    public float speed = 5f;
    public float stoppingDistance = 1f;
    public float lookAtRange = 90f;
    public Material invisibleMaterial;
    public Material visibleMaterial;

    private Renderer renderer;
    private Vector3 spawnPoint;

    private void Start()
    {
        renderer = GetComponent<Renderer>();
        spawnPoint = transform.position;
    }

    private void Update()
    {
        Vector3 direction = player.position - transform.position;
        float angle = Vector3.Angle(direction, transform.forward);

        if (direction.magnitude > stoppingDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
            renderer.material = visibleMaterial;
        }
        else
        {
            renderer.material = invisibleMaterial;
        }

        if (angle < lookAtRange)
        {
            transform.LookAt(player);
        }
    }

    private void OnBecameInvisible()
    {
        transform.position = spawnPoint;
    }
}
