using UnityEngine;
using System.Collections;

public class AnimationAutoDestroy : MonoBehaviour
{
    public float delay = 0f;
    public bool destroyParent;
    void Start()
    {
        if(transform.parent != null && destroyParent)
        {
            Destroy(transform.parent.gameObject, GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length + delay);
        }
        else
        {
            Destroy(gameObject, GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length + delay);
        }


    }
}