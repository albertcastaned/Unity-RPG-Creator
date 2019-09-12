using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowLeader : MonoBehaviour
{

    public Movement target;

    public int offset = 1;

    public float speed;


    void Update()
    {
        //TODO Agregar animaciones
        if (target.GetWaypointSize() <= offset) return;

        Vector3 destination = target.GetWayPoint(offset);

         if(transform.position != destination)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, Time.deltaTime * speed);
        }
    }
}
