using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntitySelector : MonoBehaviour
{
    private Vector3 velocity = Vector3.zero;
    private Vector3 destination;
    public float moveSpeed = 1f;


    void Update()
    {
        transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, 1 / moveSpeed);
    }
    
    public void SetDestination(Vector3 dest)
    {
        destination = dest;
    }
}
