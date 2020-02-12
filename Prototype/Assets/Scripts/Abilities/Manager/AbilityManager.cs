using UnityEngine;
using System.Collections;
using System;

public class AbilityManager : MonoBehaviour
{
    const int numAbilities = 8;
    string teamName;

    int currentAbilityIndex;

    Ability[] abilities;
    GameObject[] spellIndicators;

    Ability currentAbility;
    GameObject currentSpellIndicator;

    // Use this for initialization
    void Start()
    {
        //TODO: Load all abilities and set their casterTeamName to teamName and set the to be inactive
        // Also set the correct spell indicators for the player
        for (int i = 0; i < numAbilities; i++)
        {
            spellIndicators[i].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCooldowns();
    }

    // Set the current ability based on 
    public void SetCurrentAbility(int index)
    {
        currentAbility = abilities[index];
        currentSpellIndicator = spellIndicators[index];
        currentSpellIndicator.SetActive(true);
    }

    public void CastAbility()
    {
        if(!currentAbility.isCharging)
        {
            currentSpellIndicator.SetActive(false);
            currentAbility.Cast();
        }
        
    }

    public bool isCurrentAbilityInstant()
    {
        return currentAbility.isInstant;
    }

    public bool isCurrentAbilityCharging()
    {
        return currentAbility.isCharging;
    }

    // Check if an ability is already selected
    public bool isAbilitySelected(int index)
    {
        // Ability at position index is already selected
        if (Array.IndexOf(abilities, currentAbility) == index)
            return true;

        return false;
    }

    void UpdateCooldowns()
    {
        for (int i = 0; i < numAbilities; i++)
        {
            if(abilities[i].isCharging)
                abilities[i].UpdateCooldown(Time.deltaTime);
        }
    }
}
