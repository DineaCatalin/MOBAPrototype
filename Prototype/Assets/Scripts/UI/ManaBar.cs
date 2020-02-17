using UnityEngine.UI;
using UnityEngine;

// This will be used by both health and mana bar 
public class ManaBar : MonoBehaviour
{
    [SerializeField] Slider slider;

    private void Awake()
    {
        Debug.Log("Mana bar init");

        if (slider == null)
            slider = GetComponent<Slider>();
    }

    public void SetMaxMana(int maxMana)
    {
        slider.maxValue = maxMana;
        slider.value = maxMana;
    }

    public void SetCurrentMana(int mana)
    {
        Debug.Log("Mana setting mana to " + mana);
        slider.value = mana;
    }
}
