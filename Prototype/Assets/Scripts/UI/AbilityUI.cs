using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilityUI : MonoBehaviour
{
    public Image darkMask;
    public Image abilityIcon;

    public TextMeshProUGUI abilityName;
    public TextMeshProUGUI cooldownText;

    float cooldown;

    public void UpdateCooldown(float currentCooldown)
    {
        float roundedCooldown = Mathf.Round(currentCooldown) + 1f;
        cooldownText.text = roundedCooldown.ToString();
        darkMask.fillAmount = currentCooldown / cooldown;
    }

    public void Load(AbilityData data)
    {
        cooldown = data.stats.cooldown;
        abilityName.text = data.description.name;

        cooldownText.text = cooldown.ToString();
    }

    public void ActivateCooldown()
    {
        darkMask.enabled = true;
        cooldownText.enabled = true;
    }

    public void StopCooldown()
    {
        darkMask.enabled = false;
        cooldownText.enabled = false;
    }
}
