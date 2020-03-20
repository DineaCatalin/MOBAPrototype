﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class AbilityDraftLogic : MonoBehaviour
{
    public static AbilityDraftLogic Instance;

    // Reference so that we can change the UI in the selected abilities column in the middle of the screen
    public TextMeshProUGUI[] selectedAbilityTexts;
    string attackAbilityText;
    string defenseAbilityText;
    public TextMeshProUGUI helperText;

    public const string unselectedAbilityName = "No ability selected";

    public string ABILITY_ALREADY_SELETED = "Ability is already selected";
    public string ABILITIES_FULL = "Abilities full. Remove first";
    public string ABILITIES_NOT_SET = "Not all Abilities have been selected";

    AbilityMapperData selectedAbilityData;

    string currentAbilityName;

    // We use this so that we don't create a new TextMeshProUGUI we call DeselectAbility()
    TextMeshProUGUI cachedText;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        attackAbilityText = string.Empty;
        defenseAbilityText = string.Empty;

        selectedAbilityData = new AbilityMapperData();

        for (int i = 0; i < selectedAbilityTexts.Length; i++)
        {
            selectedAbilityTexts[i].text = unselectedAbilityName;
        }
    }

    // User presses 1 of the ability buttons on the sides of the screen
    public bool SelectSpecialbility(string abilityName)
    {
        //currentAbilityName = EventSystem.current.currentSelectedGameObject.name;
        Debug.Log("AbilityDraftLogic SelectAbility Ability name is " + abilityName);

        // Check if ability is selected
        if (AbilityIsSelected(abilityName))
        {
            helperText.text = ABILITY_ALREADY_SELETED;
            return false;
        }

        // Insert ability in the 1st free spot
        return InsertAbility(abilityName);
    }

    public bool SelectAttackAbility(string abilityName)
    {
        if (attackAbilityText.Equals(abilityName))
        {
            // Ability is already selected
            helperText.text = ABILITY_ALREADY_SELETED;
            return false;
        }

        attackAbilityText = abilityName;
        helperText.text = "";
        return true;
    }

    public bool SelectDefenseAbility(string abilityName)
    {
        if (attackAbilityText.Equals(abilityName))
        {
            // Ability is already selected
            helperText.text = ABILITY_ALREADY_SELETED;
            return false;
        }

        defenseAbilityText = abilityName;
        helperText.text = "";
        return true;
    }

    public bool AbilityIsSelected(string abilityName)
    {
        for (int i = 0; i < selectedAbilityTexts.Length; i++)
        {
            if(selectedAbilityTexts[i].text.Equals(abilityName))
            {
                Debug.Log("AbilityDraftLogic AbilityIsSelected ability " + abilityName + "  is already selected");
                return true;
            }
        }

        Debug.Log("AbilityDraftLogic AbilityIsSelected ability " + abilityName + "  is NOT selected");
        return false;
    }

    bool InsertAbility(string abilityName)
    {
        for (int i = 0; i < selectedAbilityTexts.Length; i++)
        {
            if (selectedAbilityTexts[i].text.Equals(unselectedAbilityName))
            {
                selectedAbilityTexts[i].text = abilityName;
                helperText.text = "";
                Debug.Log("AbilityDraftLogic InsertAbility inserting ability at index " + i);
                return true;
            }
        }

        helperText.text = ABILITIES_FULL;
        Debug.Log("AbilityDraftLogic InsertAbility no free space to add ability");
        return false;
    }

    // Will be triggered when a user presses one of the Selected Ability Buttons
    public void DeselectAbility()
    {
        Debug.Log("AbilityDraftLogic DeselectAbility ");
        cachedText = EventSystem.current.currentSelectedGameObject.GetComponentInChildren<TextMeshProUGUI>();
        Debug.Log("AbilityDraftLogic DeselectAbility " + cachedText.text);

        if(cachedText.text != unselectedAbilityName)
        {
            cachedText.text = unselectedAbilityName;
            helperText.text = "";
            EventManager.TriggerEvent("SpecialAbilityDeselected");
        }
    }

    public void FinishAbilitySelection()
    {
        if(attackAbilityText == string.Empty || defenseAbilityText == string.Empty)
        {
            Debug.Log("AbilityDraftLogic FinishAbilitySelection All abilities have not been selected");
            helperText.text = ABILITIES_NOT_SET;
            return;
        }

        for (int i = 0; i < selectedAbilityTexts.Length; i++)
        {
            if (selectedAbilityTexts[i].text.Equals(unselectedAbilityName))
            {
                Debug.Log("AbilityDraftLogic FinishAbilitySelection All abilities have not been selected");
                helperText.text = ABILITIES_NOT_SET;
                return;
            }
        }

        SetAbilityData();

        EventManager.TriggerEvent("DraftFinished");
        
        gameObject.SetActive(false);
        Destroy(this.gameObject);
    }

    void SetAbilityData()
    {
        selectedAbilityData.ability1 = attackAbilityText;
        selectedAbilityData.ability2 = defenseAbilityText;
        selectedAbilityData.ability3 = "Blink";

        for (int i = 0; i < selectedAbilityTexts.Length; i++)
        {
            switch (i+1)
            {
                case 1:
                    {
                        selectedAbilityData.ability4 = selectedAbilityTexts[i].text;
                        break;
                    }
                case 2:
                    {
                        selectedAbilityData.ability5 = selectedAbilityTexts[i].text;
                        break;
                    }
                case 3:
                    {
                        selectedAbilityData.ability6 = selectedAbilityTexts[i].text;
                        break;
                    }
                case 4:
                    {
                        selectedAbilityData.ability7 = selectedAbilityTexts[i].text;
                        break;
                    }
                default:
                    {
                        Debug.LogError("AbilityDraftLogic SetAbilityData out of range");
                        break;
                    }
            }
        }

        // Set the selected abilities so that the
        Debug.Log("AbilityDraftLogic SetAbilityData Setting AbilityMapper");
        AbilityMapper.SetData(selectedAbilityData);
    }

}