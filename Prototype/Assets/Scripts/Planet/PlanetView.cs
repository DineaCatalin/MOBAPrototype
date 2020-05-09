using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FadeObjectTween), typeof(ScaleTween))]
public class PlanetView : MonoBehaviour
{
    public PlanetState state;

    FadeObjectTween fadeTween;
    ScaleTween scaleTween;

    Vector2 initialScale;

    private void Awake()
    {
        fadeTween = GetComponent<FadeObjectTween>();
        scaleTween = GetComponent<ScaleTween>();
    }

    public void FadeIn(float duration, LeanTweenType easeType = LeanTweenType.linear)
    {
        fadeTween.FadeIn(duration, easeType);
    }

    public void FadeOut(float duration, LeanTweenType easeType = LeanTweenType.linear)
    {
        fadeTween.FadeOut(duration, easeType);
    }

    public void Scale(Vector2 initialScale, Vector2 finalScale, float scaleTime, LeanTweenType easeType = LeanTweenType.linear)
    {
        scaleTween.initialScale = initialScale;
        scaleTween.finalScale = finalScale;
        scaleTween.scaleTime = scaleTime;
        scaleTween.easeType = easeType;

        scaleTween.Execute();
    }
}
