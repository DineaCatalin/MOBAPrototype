using UnityEngine.UI;
using UnityEngine;

// This will be used by both health and mana bar 
public class HealthBar : MonoBehaviour
{
    [SerializeField] Slider slider;

    private void Start()
    {
        if (slider == null)
            slider = GetComponent<Slider>();
    }

    public void SetMaxHealth(int maxHealth)
    {
        slider.maxValue = maxHealth;
        slider.value = maxHealth;
    }

    public void SetCurrentHealth(int health)
    {
        slider.value = health;
    }
}
