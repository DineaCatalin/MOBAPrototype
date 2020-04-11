using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulkScaleTween : MonoBehaviour
{
    public Vector2 initialScale;
    public Vector2 finalScale;
    public float scaleTime;
    public LeanTweenType easeType;

    public GameObject[] tweenedObjects;

    private void Awake()
    {
        SetInitialScale();
    }

    // Start is called before the first frame update
    void OnEnable()
    {
        foreach (GameObject tweenedObj in tweenedObjects)
        {
            LeanTween.scale(tweenedObj, finalScale, scaleTime).setEase(easeType);
        }
    }

    private void OnDisable()
    {
        SetInitialScale();
    }

    void SetInitialScale()
    {
        foreach (GameObject tweenedObj in tweenedObjects)
        {
            tweenedObj.transform.localScale = initialScale;
        }
    }
}
