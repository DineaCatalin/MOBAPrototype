using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FadeInTextTween : Tween
{
    public float initialAlpha;
    public float finalAlpha;
    public float duration;
    public LeanTweenType easeType;

    public float delay;

    Color color;
    Color fadeInColor;

    TextMeshProUGUI text;

    // Start is called before the first frame update
    void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        color = text.color;
        fadeInColor = text.color;
        fadeInColor.a = 0;
    }

    public override void Execute()
    {
        UpdateTextColor(fadeInColor);
        LeanTween.value(gameObject, UpdateTextColor, fadeInColor, color, duration).setDelay(delay).setEase(easeType);
    }

    void UpdateTextColor(Color val)
    {
        text.color = val;
    }
}
