using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeObjectTween : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    Color color;

    float zeroAlpha = 0f;
    float fullAlpha = 1f;

    // Start is called before the first frame update
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        color = spriteRenderer.color;
    }

    public void FadeIn(float duration, LeanTweenType easeType = LeanTweenType.linear)
    {
        LeanTween.value(gameObject, setSpriteAlpha, zeroAlpha, fullAlpha, duration).setEase(easeType);
    }

    public void FadeOut(float duration, LeanTweenType easeType = LeanTweenType.linear)
    {
        LeanTween.value(gameObject, setSpriteAlpha, fullAlpha, zeroAlpha, duration).setEase(easeType);
    }

    void setSpriteAlpha(float val)
    {
        color.a = val;
        spriteRenderer.color = color;
    }
}
