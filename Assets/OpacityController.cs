using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(SpriteRenderer))]
public class OpacityController : MonoBehaviour
{
    private Color color;

    void Start()
    {
        color = GetComponent<SpriteRenderer>().color;

        StartCoroutine(nameof(InitiatePrefabAnimation));
    }

    public IEnumerator InitiatePrefabAnimation()
    {
        color = new Color(1f, 1f, 1f, 0f);

        Color col = new Color(1f,1f,1f,1f);

        float t = 0;
        while (t < 1)
        {
            color = Color.Lerp(color, col, t);
            t += Time.deltaTime / 160f;

            yield return null;
        }
    }
}
