using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class MenuSlotController : MonoBehaviour
{

    public bool ACTIVE;
    public bool DISABLED;

    private bool inPosition;

    public bool ChangePosition = true;
    [SerializeField]
    private Vector3 UnactivePosition;
    [SerializeField]
    private Vector3 ActivePosition;

    public bool ChangeScale;
    [SerializeField]
    private Vector3 UnactiveScale = new Vector3(1f,1f,1f);
    [SerializeField]
    private Vector3 ActiveScale;
    
    private Vector3 velocity = Vector3.zero;

    public float moveSpeed = 1f;


    public bool relative;

    private Vector3 originalPosition;

    public TextMeshProUGUI text;
    void Start()
    {
        if (!relative) return;
        originalPosition = transform.localPosition;
        ActivePosition = originalPosition + ActivePosition;
        UnactivePosition = originalPosition + UnactivePosition;

    }

    void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();

    }



    public void SetToDisabled()
    {
        DISABLED = true;
        text.color = new Color(133f / 255f, 133f / 255f, 133f / 255f);
    }
    public void Activate()
    {
        if(!DISABLED)
            text.color = new Color(0.96f, 1f, 0.43f);
        if (ChangePosition)
        {
            StopCoroutine(nameof(MoveToActive));
            StopCoroutine(nameof(MoveToUnactive));

            StartCoroutine(nameof(MoveToActive));
        }

        if (ChangeScale)
        {
            StopCoroutine(nameof(ScaleToUnactive));
            StopCoroutine(nameof(ScaleToActive));

            StartCoroutine(nameof(ScaleToActive));
        }
    }

    public void Deactivate()
    {
        if(!DISABLED)
            text.color = Color.white;

        if (ChangePosition)
        {
            StopCoroutine(nameof(MoveToActive));
            StopCoroutine(nameof(MoveToUnactive));

            StartCoroutine(nameof(MoveToUnactive));
        }

        if (ChangeScale)
        {
            StopCoroutine(nameof(ScaleToActive));
            StopCoroutine(nameof(ScaleToUnactive));

            StartCoroutine(nameof(ScaleToUnactive));
        }


    }
    
    private IEnumerator ScaleToActive()
    {

        while(!V3Equal(transform.localScale, ActiveScale))
        {
            transform.localScale = Vector3.SmoothDamp(transform.localScale, ActiveScale, ref velocity, 1 / moveSpeed);
            yield return null;
        }
    }
    
    private IEnumerator ScaleToUnactive()
    {

        while(!V3Equal(transform.localScale, UnactiveScale))
        {
            transform.localScale = Vector3.SmoothDamp(transform.localScale, UnactiveScale, ref velocity, 1 / moveSpeed);
            yield return null;
        }
    }
    private IEnumerator MoveToActive()
    {
            
            while(!V3Equal(transform.localPosition, ActivePosition))
            {
                transform.localPosition = Vector3.SmoothDamp(transform.localPosition, ActivePosition, ref velocity, 1 / moveSpeed);
                yield return null;
            }
    }

    private IEnumerator MoveToUnactive()
    {

        while (!V3Equal(transform.localPosition, UnactivePosition))
        {
            transform.localPosition = Vector3.SmoothDamp(transform.localPosition, UnactivePosition, ref velocity, 1 / moveSpeed);
            yield return null;

        }


    }

    private static bool V3Equal(Vector3 a, Vector3 b)
    {
        return Vector3.SqrMagnitude(a - b) < 0.00005;
    }

}
