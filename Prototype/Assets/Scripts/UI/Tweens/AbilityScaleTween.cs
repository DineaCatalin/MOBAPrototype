using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityScaleTween : MonoBehaviour
{
    public Vector3 initialScale;
    public Vector3 finalScale;
    public float scaleTime;
    public LeanTweenType easeType;

    void OnEnable()
    {
        gameObject.transform.localScale = initialScale;
        LeanTween.scale(gameObject, finalScale, scaleTime).setEase(easeType);
    }
}
