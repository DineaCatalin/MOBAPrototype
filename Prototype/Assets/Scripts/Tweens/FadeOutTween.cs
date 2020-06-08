using UnityEngine;

public class FadeOutTween : Tween
{
    public float finalAlpha;
    public float duration;
    public LeanTweenType easeType;

    public float delay;

    RectTransform rectTransform;

    Color color;

    // Start is called before the first frame update
    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public override void Execute()
    {
        LeanTween.alpha(rectTransform, finalAlpha, duration).setDelay(delay).setEase(easeType);
    }
}