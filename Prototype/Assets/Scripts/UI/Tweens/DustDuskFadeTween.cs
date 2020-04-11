using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustDuskFadeTween : MonoBehaviour
{
    public float objectVisibleTime;

    public float initialAlpha;
    public float finalAlpha;
    public float duration;
    public LeanTweenType easeType;

    SpriteRenderer spriteRenderer;

    Color color;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        color = spriteRenderer.color;

        objectVisibleTime = AbilityDataCache.GetDataForAbility(name).stats.duration;

        setSpriteAlpha(initialAlpha);
    }

    public void Fade()
    {
        Debug.Log("FadeInOutTween OnEnable reseting alpha in " + objectVisibleTime + " - " + duration);
        
        LeanTween.value(gameObject, setSpriteAlpha, initialAlpha, finalAlpha, duration).setEase(easeType);
        LeanTween.value(gameObject, setSpriteAlpha, finalAlpha, initialAlpha, duration).setDelay(objectVisibleTime - duration).setEase(easeType);
    }

    void setSpriteAlpha(float val)
    {
        color.a = val;
        spriteRenderer.color = color;
    }
}
