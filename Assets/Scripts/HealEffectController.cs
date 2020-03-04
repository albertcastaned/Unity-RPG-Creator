using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealEffectController : MonoBehaviour
{
    private const float speed = 6f;
    public Transform origin;
    private float angle;
    public GameObject healParticle;
    private float opacity = 1f;

    private new SpriteRenderer renderer;
    public bool ready;


    void Start()
    {
        angle = Random.Range(-10f, 10f);
        renderer = GetComponent<SpriteRenderer>();


    }

    public void Setup(Transform transf)
    {
        var position = transf.position;
        transform.position = new Vector3(position.x + 1.5f, position.y + 1f, position.z);
        origin = transf;


        ready = true;
    }

    void Update()
    {
        if (!ready)
            return;
        if (Random.Range(0, 100) < (30 - (1 / opacity) / 2))
        {
            Instantiate(healParticle, new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation);
        }

        var aux = Quaternion.Euler(angle, 0, 0) * origin.up * 5;
        transform.RotateAround(origin.position, aux, speed);
        opacity -= Time.deltaTime / 2f;
        var color = renderer.color;
        color = new Color(color.r, color.g, color.b, opacity);
        renderer.color = color;

        if (opacity <= 0)
        {
            Destroy(gameObject);
        }
    }
}
