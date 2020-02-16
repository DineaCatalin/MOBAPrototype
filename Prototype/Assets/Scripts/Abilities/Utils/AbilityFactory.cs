﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class AbilityFactory : MonoBehaviour
{
    public static AbilityFactory SharedInstance;

    // We will fill this from the inspector will all the Ability GameObjects from the Scene 
    [SerializeField] Ability[] abilityContainerArray;

    // We will move the ability GOs here for easier access
    Dictionary<string, Ability> abilityMap;

    Ability templateAbility;

    AbilityMapper mapper;

    private void Awake()
    {
        SharedInstance = this;
    }

    private void Start()
    {
        mapper = new AbilityMapper();
        abilityMap = new Dictionary<string, Ability>();

        // Fill the AbilityMap
        for (int i = 0; i < abilityContainerArray.Length; i++)
        {
            Debug.Log("AbilityFactory Adding ability with name " + abilityContainerArray[i].name);
            abilityMap.Add(abilityContainerArray[i].name, abilityContainerArray[i]);
        }
    }

    public Ability[] GetCurrentAbilities()
    {
        Ability[] currentAbilities = new Ability[8];

        string abilityName;

        for (int i = 0; i < 8; i++)
        {
            // Get 1st ability
            abilityName = mapper.GetAbilityNameForIndex(i + 1); // i+1 because array is 0 indexed and mapper is 1 indexed

            // Spawn it and add it to the list
            currentAbilities[i] = CreateAbility(abilityName);
        }

        return currentAbilities;
    }

    Ability CreateAbility(string abilityname)
    {
        templateAbility = GetAbility(abilityname);

        return Instantiate(templateAbility);
    }

    Ability GetAbility(string key)
    {
        Debug.Log("AbilityFactory key before " + key);
        Regex.Replace(key, @"\s+", "");
        key += "Ability";
        key.Replace(" ", "");
        Regex.Replace(key, @"\s+", "");
        Debug.Log("AbilityFactory key after  " + key);
        return abilityMap[key];
    }
}
