using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class AbilityDraftLogic : MonoBehaviour
{
    // Reference so that we can change the UI in the selected abilities column in the middle of the screen
    public TextMeshProUGUI[] selectedAbilityTexts;
    public const string unselectedAbilityName = "No ability selected";

    AbilityMapperData selectedAbilityData;

    private void Start()
    {
        selectedAbilityData = new AbilityMapperData();

        for (int i = 0; i < selectedAbilityTexts.Length; i++)
        {
            selectedAbilityTexts[i].text = unselectedAbilityName;
        }
    }

    // User presses 1 of the ability buttons on the sides of the screen
    public void SelectAbility()
    {
        string currentAbilityName = EventSystem.current.currentSelectedGameObject.name;
        Debug.Log("AbilityDraftLogic SelectAbility Ability name is " + currentAbilityName);

        // Check if ability is selected
        if (AbilityIsSelected(currentAbilityName))
            return;

        // Insert ability in the 1st free spot
        InsertAbility(currentAbilityName);
    }

    bool AbilityIsSelected(string abilityName)
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

    void InsertAbility(string abilityName)
    {
        for (int i = 0; i < selectedAbilityTexts.Length; i++)
        {
            if (selectedAbilityTexts[i].text.Equals(unselectedAbilityName))
            {
                selectedAbilityTexts[i].text = abilityName;
                Debug.Log("AbilityDraftLogic InsertAbility inserting ability at index " + i);
                return;
            }
        }

        Debug.Log("AbilityDraftLogic InsertAbility no free space to add ability");
    }

    // Will be triggered when a user presses one of the Selected Ability Buttons
    public void DeselectAbility()
    {
        Debug.Log("AbilityDraftLogic DeselectAbility ");
        Debug.Log("AbilityDraftLogic DeselectAbility " + EventSystem.current.currentSelectedGameObject.GetComponentInChildren<TextMeshProUGUI>().text);
        EventSystem.current.currentSelectedGameObject.GetComponentInChildren<TextMeshProUGUI>().text = unselectedAbilityName;
    }

    public void FinishAbilitySelection()
    {
        for (int i = 0; i < selectedAbilityTexts.Length; i++)
        {
            if (selectedAbilityTexts[i].text.Equals(unselectedAbilityName))
            {
                Debug.Log("AbilityDraftLogic FinishAbilitySelection All abilities have not been selected");

                // Also change helper text here
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
        for (int i = 0; i < selectedAbilityTexts.Length; i++)
        {
            switch (i+1)
            {
                case 1:
                    {
                        selectedAbilityData.ability1 = selectedAbilityTexts[i].text;
                        break;
                    }
                case 2:
                    {
                        selectedAbilityData.ability2 = selectedAbilityTexts[i].text;
                        break;
                    }
                case 3:
                    {
                        selectedAbilityData.ability3 = selectedAbilityTexts[i].text;
                        break;
                    }
                case 4:
                    {
                        selectedAbilityData.ability4 = selectedAbilityTexts[i].text;
                        break;
                    }
                case 5:
                    {
                        selectedAbilityData.ability5 = selectedAbilityTexts[i].text;
                        break;
                    }
                case 6:
                    {
                        selectedAbilityData.ability6 = selectedAbilityTexts[i].text;
                        break;
                    }
                case 7:
                    {
                        selectedAbilityData.ability7 = selectedAbilityTexts[i].text;
                        break;
                    }
                case 8:
                    {
                        selectedAbilityData.ability8 = selectedAbilityTexts[i].text;
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
        AbilityMapper.SetData(selectedAbilityData);
    }

}
