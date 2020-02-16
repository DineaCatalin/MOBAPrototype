using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivationDelay : MonoBehaviour
{
    [SerializeField] float delay = 10f;

    SpriteRenderer spriteRenderer;
    CircleCollider2D circleCollider;

    Color color;

    // Update is called once per frame
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        circleCollider = GetComponent<CircleCollider2D>();

        StartCoroutine("Activate");
    }

    IEnumerator Activate()
    {
        yield return new WaitForSeconds(delay);

        circleCollider.enabled = true;

        // Set alpha to max to the object will be opaque
        color = spriteRenderer.color;
        color.a = 1;
        spriteRenderer.color = color;
    }

}
