using UnityEngine;
using System.Collections;
using System;

public class AbilityManager : MonoBehaviour
{
    const int numAbilities = 7;

    int currentAbilityIndex;

    // Insert all the Ability UIs in the inspector
    public AbilityUI[] abilityUIs;

    Ability[] abilities;

    GameObject[] spellIndicators;

    Ability currentAbility;

    Player player;

    // Use this for initialization
    void Start()
    {
        abilities = AbilityFactory.SharedInstance.GetCurrentAbilities();

        player = GetComponent<Player>();

        int playerID = player.GetID();

        spellIndicators = new GameObject[abilities.Length];

        AbilityUI[] uis = AbilityUIContainer.Instance.abilitieUIs;

        // Load all abilities and set the to be inactive
        // Also set the correct spell indicators for the player
        for (int i = 0; i < abilities.Length; i++)
        {
            currentAbility = abilities[i];
            currentAbility.Load();
            currentAbility.SetUI(uis[i]);
            currentAbility.SetPlayerID(playerID);
            currentAbility.gameObject.layer = player.gameObject.layer;

            spellIndicators[i] = Instantiate(currentAbility.PrepareSpellIndicator());
            spellIndicators[i].transform.parent = this.transform;
            spellIndicators[i].transform.localPosition = new Vector3(0,0,1);

            // Give the instantiated spell indicator also to the ability so it can use it later
            currentAbility.SetSpellIndicator(spellIndicators[i]);
            spellIndicators[i].SetActive(false);
        }
        
        currentAbility = null;
        currentAbilityIndex = -1;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("AbilityManager Update()");
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
            currentAbility = null;
            currentAbilityIndex = -1;
            spellIndicators[index].SetActive(false);
            return;
        }

        // Cast if the ability is instant
        if (abilities[index].isInstant)
        {
            currentAbility = abilities[index];
            currentAbilityIndex = index;
            CastAbility();
            return;
        }
            
        // Disable old spell indicator if it is stil active
        if(currentAbilityIndex != -1)
        {
            spellIndicators[currentAbilityIndex].SetActive(false);
        }
            

        currentAbilityIndex = index;
        currentAbility = abilities[index];
        spellIndicators[index].SetActive(true);
    }
    
    public void CastAbility()
    {
        Debug.Log("AbilityManager CastAbility ability " + currentAbility.name + " isCharging " + currentAbility.IsCharging());

        if(!currentAbility.IsCharging())
        {
            // Check if we have enough mana
            if (!EnoughManaForSelectedAbility())
            {
                // Not enough mana
                Debug.Log("NOT ENOUGH MANA");
                return;
            }

            // If the ability was cast with success
            if(currentAbility.Cast())
            {
                Debug.Log("AbilityManager CastAbility ability has been cast " + currentAbility.name);
                player.UseMana(currentAbility.GetManaCost());
                DeselectAbility();
                return;
            }
        }

    }

    public bool IsCurrentAbilityInstant()
    {
        return currentAbility.isInstant;
    }

    public bool IsCurrentAbilityCharging()
    {
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
    public bool IsAbilitySelected(int index)
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
            abilities[i].UpdateCooldown();
        }
    }

    void DeselectAbility()
    {
        spellIndicators[currentAbilityIndex].SetActive(false);
        currentAbility.ResetCharging();
        currentAbility = null;
        currentAbilityIndex = -1;
    }

    bool EnoughManaForSelectedAbility()
    {
        return player.EnoughManaForAbility(currentAbility.GetManaCost());
    }
}
