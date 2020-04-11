using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleTween : MonoBehaviour
{
    public Vector2 initialScale;
    public Vector2 finalScale;
    public float scaleTime;
    public LeanTweenType easeType;

    void OnEnable()
    {
        SetInitialScale();
        LeanTween.scale(gameObject, finalScale, scaleTime).setEase(easeType);
    }

    void SetInitialScale()
    {
        gameObject.transform.localScale = initialScale;
    }
}
