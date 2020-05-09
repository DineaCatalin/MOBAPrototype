using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FlashTextTween : Tween
{
	public Vector2 initialScale;
	public Vector2 finalScale;
	public float duration;
	public LeanTweenType easeType;

	int alphaMin = 0;
	int alphaMax = 1;

	TextMeshProUGUI text;

	Color color;

    bool locked;

	// Start is called before the first frame update
	void Awake()
	{
		text = GetComponent<TextMeshProUGUI>();
		color = text.color;
	}

	void SetSpriteAlpha(float val)
	{
		color.a = val;
		text.color = color;
	}

	public override void Execute()
	{
        locked = true;

		gameObject.transform.localScale = initialScale;

		LeanTween.scale(gameObject, finalScale, duration).setEase(easeType);
		LeanTween.value(gameObject, SetSpriteAlpha, alphaMin, alphaMax, duration / 2).setEase(easeType);
		LeanTween.value(gameObject, SetSpriteAlpha, alphaMax, alphaMin, duration / 2).setDelay(duration / 2).setEase(easeType).setOnComplete(Unlock);

    }

    void Unlock()
    {
        locked = false;
    }
}
