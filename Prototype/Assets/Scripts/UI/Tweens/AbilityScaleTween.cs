using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityScaleTween : MonoBehaviour
{
    public Vector2 initialScale;
    public Vector2 finalScale;
    public float scaleTime;
    public LeanTweenType easeType;

    void OnEnable()
    {
        gameObject.transform.localScale = initialScale;
        LeanTween.scale(gameObject, finalScale, scaleTime).setEase(easeType);
    }
}
