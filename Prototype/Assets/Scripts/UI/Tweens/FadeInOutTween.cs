using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInOutTween : MonoBehaviour
{
    public float initialAlpha;
    public float finalAlpha;
    public float totalDuration;
    public float fadeDuration;
    public LeanTweenType easeType;

    SpriteRenderer spriteRenderer;

    Color color;

    // Start is called before the first frame update
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>() as SpriteRenderer;
        color = spriteRenderer.color;
    }

    public void OnEnable()
    {
        setSpriteAlpha(initialAlpha);
        LeanTween.value(gameObject, setSpriteAlpha, initialAlpha, finalAlpha, fadeDuration).setEase(easeType);
        LeanTween.value(gameObject, setSpriteAlpha, finalAlpha, initialAlpha, fadeDuration).setDelay(totalDuration - fadeDuration).setEase(easeType);
    }

    void setSpriteAlpha(float val)
    {
        color.a = val;
        spriteRenderer.color = color;
    }
}
