using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{

    public bool ACTIVE;

    [SerializeField]
    private Vector3 UnactivePosition;
    [SerializeField]
    private Vector3 ActivePosition;

    private Vector3 velocity = Vector3.zero;

    public float moveSpeed = 1f;
    void Update()
    {
        if (ACTIVE)
            transform.localPosition = Vector3.SmoothDamp(transform.localPosition, ActivePosition, ref velocity, 1 / moveSpeed);
        else
            transform.localPosition = Vector3.SmoothDamp(transform.localPosition, UnactivePosition, ref velocity, 1 / moveSpeed);
    }
}
