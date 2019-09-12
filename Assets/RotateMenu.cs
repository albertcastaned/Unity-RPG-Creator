using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateMenu : MonoBehaviour
{

    private Vector3 EulerRotation = new Vector3(0f, 60f, 0f);
    public float RotateSpeed = 0.2f;
    private Vector3 velocity = Vector3.zero;
    private bool rotating;
    // Update is called once per frame
    void Update()
    {
        if (rotating)
            return;
        if (Input.GetKey(KeyCode.D))
        {
            StartCoroutine(Rotate(EulerRotation));
        }
        if (Input.GetKey(KeyCode.A))
        {
            StartCoroutine(Rotate(-EulerRotation));
        }
    }
    private IEnumerator Rotate(Vector3 rotation)
    {

        rotating = true;
        var start = transform.rotation;
        var destination = start * Quaternion.Euler(rotation);
        var startTime = Time.time;
        var percentComplete = 0f;
        while (percentComplete <= 1.0f)
        {
            percentComplete = (Time.time - startTime) / RotateSpeed;
            transform.rotation = Quaternion.Slerp(start, destination, percentComplete);
            yield return null;
        }
        transform.rotation = destination;
        rotating = false;
    }

}
