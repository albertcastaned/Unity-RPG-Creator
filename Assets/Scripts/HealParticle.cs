using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealParticle : MonoBehaviour
{
    private new SpriteRenderer renderer;
    private float opacity = 1f;
    private float ranSpeed;

    void Start()
    {
        ranSpeed = Random.Range(0f, 0.05f);
        renderer = GetComponent<SpriteRenderer>();
        var eulerAngles = transform.eulerAngles;
        eulerAngles = new Vector3(eulerAngles.x, eulerAngles.y, eulerAngles.z + Random.Range(-20f, 20f));
        transform.eulerAngles = eulerAngles;
    }

    void Update()
    {
        var position = transform.position;
        position = new Vector3(position.x, position.y - ranSpeed, position.z);
        transform.position = position;
        renderer.material.color = new Color(1f, 1f, 1f, opacity);
        opacity -= Time.deltaTime / 5f;

        if (opacity <= 0)
            Destroy(gameObject);
    }
}
