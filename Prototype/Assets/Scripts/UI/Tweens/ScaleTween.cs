using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleTween : MonoBehaviour
{
    public Vector2 initialScale;
    public Vector2 finalScale;
    public float scaleTime;
    public LeanTweenType easeType;

    private void Awake()
    {
        SetInitialScale();
    }

    // Start is called before the first frame update
    void OnEnable()
    {    
        LeanTween.scale(gameObject, finalScale, scaleTime).setEase(easeType);
    }

    private void OnDisable()
    {
        SetInitialScale();
    }

    void SetInitialScale()
    {
        gameObject.transform.localScale = initialScale;
    }
}
