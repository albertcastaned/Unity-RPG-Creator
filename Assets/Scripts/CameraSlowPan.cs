using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraSlowPan : MonoBehaviour
{

    private Camera cam;

    private Transform myTransform;

    public float speed;

    public float offset;

    private float camYRotation = 0f;

    void Start()
    {
        cam = GetComponent<Camera>();
        myTransform = GetComponent<Transform>();
    }

    void Update()
    {

            camYRotation = 0f + (-offset / 2f + (Mathf.PingPong(Time.time * speed, offset)));
            myTransform.localRotation = Quaternion.Euler(25f, camYRotation, 0f);

        

    }
}
