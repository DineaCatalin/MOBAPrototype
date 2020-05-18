using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInTween : Tween
{
    public float initialAlpha;
    public float finalAlpha;
    public float duration;
    public LeanTweenType easeType;

    public float delay;

    RectTransform rectTransform;

    Color color;

    // Start is called before the first frame update
    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public override void Execute()
    {
        LeanTween.alpha(rectTransform, initialAlpha, 0f);
        LeanTween.alpha(rectTransform, finalAlpha, duration).setDelay(delay).setEase(easeType);
    }
}
