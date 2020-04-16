using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleTween : Tween
{
    public Vector2 initialScale;
    public Vector2 finalScale;
    public float scaleTime;
    public LeanTweenType easeType;

    public override void Execute()
    {
        gameObject.transform.localScale = initialScale;
        LeanTween.scale(gameObject, finalScale, scaleTime).setEase(easeType);
    }
}
