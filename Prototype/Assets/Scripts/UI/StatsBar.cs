using UnityEngine.UI;
using UnityEngine;

// This will be used by both health and mana bar 
public class StatsBar : MonoBehaviour
{
    [SerializeField] Slider slider;

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
        slider.value = stat;
    }
 }
