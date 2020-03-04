using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ShootAtTarget : AttackAnimationController
{

    private Vector3 destination;
    private float speed = 15f;

    void Start()
    {
        destination = targets[0].gameObject.transform.position;
    }


    void Update()
    {

        transform.position = Vector3.MoveTowards(transform.position, destination, Time.deltaTime * speed);

        if (Vector3.Equals(transform.position, destination))
        {
            targets[0].ReceiveDamage(damage);
            FinishAttackAnimation();
        }
    }
}
