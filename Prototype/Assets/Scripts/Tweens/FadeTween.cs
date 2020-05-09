using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeTween : MonoBehaviour
{
    public float initialAlpha;
    public float finalAlpha;
    public float duration;
    public LeanTweenType easeType;

    SpriteRenderer spriteRenderer;

    Color color;

    // Start is called before the first frame update
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        color = spriteRenderer.color;
    }

    private void OnEnable()
    {
        setSpriteAlpha(initialAlpha);
        LeanTween.value(gameObject, setSpriteAlpha, initialAlpha, finalAlpha, duration).setEase(easeType);
    }

    void setSpriteAlpha(float val)
    {
        color.a = val;
        spriteRenderer.color = color;
    }
}
