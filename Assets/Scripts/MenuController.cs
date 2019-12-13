using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{

    public bool ACTIVE;

    private bool inPosition;
    
    [SerializeField]
    private Vector3 UnactivePosition;
    [SerializeField]
    private Vector3 ActivePosition;

    private Vector3 velocity = Vector3.zero;

    public float moveSpeed = 1f;

    private BobEffect bob;

    public bool relative = false;

    private Vector3 originalPosition;
    void Start()
    {
       bob = GetComponent<BobEffect>();

        if (relative)
        {
            originalPosition = transform.localPosition;
            ActivePosition = originalPosition + ActivePosition;
            UnactivePosition = originalPosition + UnactivePosition;

        }
    }
    void Update()
    {

        if (ACTIVE)
        {
            if (!inPosition)
            {
                if (!V3Equal(transform.localPosition, ActivePosition))
                {
                    transform.localPosition = Vector3.SmoothDamp(transform.localPosition, ActivePosition, ref velocity, 1 / moveSpeed);
                }
                else
                {
                    inPosition = true;
                    if (bob != null)
                    {
                        bob.SetActive(true);
                        bob.Reset();
                    }
                }
            }
        }
        else
        {
            inPosition = false;
            if (bob != null)
            {
                bob.SetActive(false);
            }
            if (!V3Equal(transform.localPosition, UnactivePosition))
            {
                transform.localPosition = Vector3.SmoothDamp(transform.localPosition, UnactivePosition, ref velocity, 1 / moveSpeed);
            }
        }
    }
    private static bool V3Equal(Vector3 a, Vector3 b)
    {
        return Vector3.SqrMagnitude(a - b) < 0.00005;
    }

}
