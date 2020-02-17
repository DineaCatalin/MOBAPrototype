using UnityEngine.UI;
using UnityEngine;

// This will be used by both health and mana bar 
public class HealthBar : MonoBehaviour
{
    [SerializeField] Slider slider;

    private void Awake()
    {
        if (slider == null)
            slider = GetComponent<Slider>();

        if (slider == null)
            Debug.Log("Slider is null wtf?");
    }

    public void SetMaxHealth(int maxHealth)
    {
        slider.maxValue = maxHealth;
        slider.value = maxHealth;
    }

    public void SetCurrentHealth(int health)
    {
        Debug.Log("HealthBar Setting health to " + health);
        slider.value = health;
    }
}
