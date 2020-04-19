using UnityEngine.UI;
using UnityEngine;

// This will be used by both health and mana bar 
public class StatsBar : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] float fullSlideDuration;
    float duration;
    [SerializeField] LeanTweenType easeType;

    private void Start()
    {
        if (slider == null)
            slider = GetComponent<Slider>();
    }

    public void SetMaxStat(int maxStat)
    {
        slider.maxValue = maxStat;
        slider.value = maxStat;
    }

    public void SetCurrentStat(int stat)
    {
        duration = fullSlideDuration * (stat / slider.maxValue);
        LeanTween.value(gameObject, SetSliderValue, slider.value, stat, duration).setEase(easeType);
    }

    void SetSliderValue(float value)
    {
        slider.value = value;
    }
}
