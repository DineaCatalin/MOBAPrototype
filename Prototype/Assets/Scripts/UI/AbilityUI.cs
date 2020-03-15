using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilityUI : MonoBehaviour
{
    public int id;

    public Image darkMask;
    public Image abilityIcon;

    public TextMeshProUGUI abilityName;
    public TextMeshProUGUI cooldownText;

    public TextMeshProUGUI abilityKey;      // Key the user has to press to release the ability 

    float cooldown;

    private void Start()
    {
        // Get the KeyCode for the ability that has the index = id 
        AbilityInputKey key = GetKeyCodeForID(id);
        KeyCode code = (KeyCode)key;

        // If the ability is a number it will the name will contain Alpha
        // For example 1 will be "Alpha1" so we will take out the Alpha substring 
        abilityKey.text = code.ToString().Replace("Alpha", "");
    }

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

    AbilityInputKey GetKeyCodeForID(int keyID)
    {
        switch(keyID)
        {
            case 1:
                {
                    return AbilityInputKey.Ability1;
                }
            case 2:
                {
                    return AbilityInputKey.Ability2;
                }
            case 3:
                {
                    return AbilityInputKey.Ability3;
                }
            case 4:
                {
                    return AbilityInputKey.Ability4;
                }
            case 5:
                {
                    return AbilityInputKey.Ability5;
                }
            case 6:
                {
                    return AbilityInputKey.Ability6;
                }
            case 7:
                {
                    return AbilityInputKey.Ability7;
                }
            case 8:
                {
                    return AbilityInputKey.Ability8;
                }

            default:
                {
                    return 0;
                }
        }
    }
}
