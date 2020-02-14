using UnityEngine;
using System.Collections;
using System;

public class AbilityManager : MonoBehaviour
{
    const int numAbilities = 8;
    string teamName;

    int currentAbilityIndex;

    [SerializeField] Ability[] abilities;

    //[SerializeField] GameObject[] projectiles;
    [SerializeField] GameObject[] spellIndicators;

    Ability currentAbility;

    // Use this for initialization
    void Start()
    {
        spellIndicators = new GameObject[abilities.Length];

        //TODO: Load all abilities and set their casterTeamName to teamName and set the to be inactive
        // Also set the correct spell indicators for the player
        for (int i = 0; i < abilities.Length; i++)
        {
            currentAbility = abilities[i];
            currentAbility.Load();

            spellIndicators[i] = Instantiate(currentAbility.PrepareSpellIndicator(), this.transform);
            spellIndicators[i].SetActive(false);
        }

        currentAbility = null;
        currentAbilityIndex = -1;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("AbilityManager Update()");
        UpdateCooldowns();
    }

    // Set the current ability based on 
    public void SetCurrentAbility(int index)
    {
        // Current ability is charging so don't do anything
        if (abilities[index].IsCharging())
            return;

        // We are choosin the same ability so toggle it off
        if (currentAbilityIndex  == index)
        {
            Debug.Log("SetCurrentAbility: Index is the same " + index);

            currentAbility = null;
            currentAbilityIndex = -1;
            spellIndicators[index].SetActive(false);
            return;
        }

        Debug.Log("SetCurrentAbility: Changing index to " + index);

        // Disable old spell indicator if it is stil active
        if(currentAbilityIndex != -1)
            spellIndicators[currentAbilityIndex].SetActive(false);


        currentAbilityIndex = index;
        currentAbility = abilities[index];
        spellIndicators[index].SetActive(true);
        
    }

    public void CastAbility()
    {
        Debug.Log("AbilityManager: CastAbility currentAbility is charging " + currentAbility.IsCharging());
        if(!currentAbility.IsCharging())
        {
            currentAbility.Cast();
            Debug.Log("AbilityManager: CastAbility After cast currentAbility is charging " + currentAbility.IsCharging());


            spellIndicators[currentAbilityIndex].SetActive(false);
            currentAbility = null;
            currentAbilityIndex = -1;
        }

    }

    public bool IsCurrentAbilityInstant()
    {
        return currentAbility.isInstant;
    }

    public bool IsCurrentAbilityCharging()
    {
        Debug.Log("AbilityManager: IsCurrentAbilityCharging " + currentAbility.IsCharging());
        return currentAbility.IsCharging();
    }

    // We use this to check if there is no ability that is selected
    public bool NoAbilitySelected()
    {
        if (currentAbility == null)
            return true;

        return false;
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
        for (int i = 0; i < abilities.Length; i++)
        {
            Debug.Log("Updating ability cooldown");
            abilities[i].UpdateCooldown();
        }
    }
}