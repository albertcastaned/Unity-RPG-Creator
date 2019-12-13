using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobEffect : MonoBehaviour
{

    public Vector3 bobStrength = Vector3.one;
    public float speed = 1f;
    private float startTime;
    private float elapsedTime;
    private Vector3 originalPosition;
    private bool ACTIVE = true;

    void Start()
    {
        originalPosition = transform.position;
        startTime = Time.time;
    }

    void Update()
    {
        
        if (!ACTIVE)
            return;
        elapsedTime = Time.time - startTime;
        transform.position = new Vector3(originalPosition.x + (float)(Mathf.Sin(elapsedTime * speed) * bobStrength.x), originalPosition.y + (float)(Mathf.Sin(elapsedTime * speed) * bobStrength.y), originalPosition.z + (float)(Mathf.Sin(elapsedTime * speed) * bobStrength.z));
    }

    public void SetActive(bool flag)
    {
        ACTIVE = flag;
    }
    public void Reset()
    {
        originalPosition = transform.position;
        startTime = Time.time;
    }
    public bool GetActive() => ACTIVE;
}
