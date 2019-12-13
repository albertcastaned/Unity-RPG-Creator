using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChoice : MonoBehaviour
{

    public PlayerMenuChoice slotData;
    private float originalScale;
    public bool GROW;
    public float MaxSize = 3f;
    public float growSpeed = 2.5f;
    void Awake()
    {
        GetComponent<SpriteRenderer>().sprite = slotData.image;
    }
    void Start()
    {
        originalScale = transform.localScale.x;

    }

    // Update is called once per frame
    void Update()
    {
        if (GROW)
        {
            if (transform.localScale.x < MaxSize)
            {

                transform.localScale = new Vector3(transform.localScale.x + (Time.deltaTime * growSpeed), transform.localScale.y + (Time.deltaTime * growSpeed), 1f);
            }
            else
            {
                transform.localScale = new Vector3(MaxSize, MaxSize, 1f);
            }
        }
        else
        {
            if(transform.localScale.x > originalScale)
            {

                transform.localScale = new Vector3(transform.localScale.x - (Time.deltaTime * growSpeed), transform.localScale.y - (Time.deltaTime * growSpeed), 1f);
            }
            else
            {
                transform.localScale = new Vector3(originalScale, originalScale, 1f);
            }
        }
    }
    public void ChangeGrow()
    {
        GROW = !GROW;
    }
}
