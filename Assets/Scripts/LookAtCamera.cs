using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{

    private Camera myCamera;
    void Start()
    {
        myCamera = GameObject.Find("MainCamera").GetComponent<Camera>();
        transform.LookAt(myCamera.transform);

        transform.eulerAngles = new Vector3(0f, -transform.eulerAngles.y, transform.eulerAngles.z);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
