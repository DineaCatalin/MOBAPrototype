using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FadeInTween),typeof(FadeOutTween))]
public class BackgroundUI : MonoBehaviour
{
    FadeInTween fadeInTween;
    FadeOutTween fadeOutTween;


    // Start is called before the first frame update
    void Awake()
    {
        fadeInTween = GetComponent<FadeInTween>();
        fadeOutTween = GetComponent<FadeOutTween>();
    }

    public void Show()
    {
        fadeInTween.Execute();
    }

    public void Hide()
    {
        fadeOutTween.Execute();
    }

    public float GetFadeInDuration()
    {
        return fadeInTween.duration;
    }

    public float GetFadeOutDuration()
    {
        return fadeOutTween.duration;
    }
}
