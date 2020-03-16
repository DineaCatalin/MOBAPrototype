using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class AbilityFactory : MonoBehaviour
{
    public static AbilityFactory SharedInstance;

    // We will fill this from the inspector will all the Ability GameObjects from the Scene 
    [SerializeField] Ability[] abilityContainerArray;

    // We will move the ability GOs here for easier access
    public Dictionary<string, Ability> abilityMap;

    Ability templateAbility;

    AbilityMapper mapper;

    private void Awake()
    {
        SharedInstance = this;

        // Load all abilities from the Resource folder
        //LoadAbilities();

        // This is for mapping the selected abilties
        mapper = new AbilityMapper();

        // This is the container of all abilities
        // We will move all abilities from an array to a map because it is faster to access them
        abilityMap = new Dictionary<string, Ability>(); 

        // Fill the AbilityMap
        for (int i = 0; i < abilityContainerArray.Length; i++)
        {
            abilityMap.Add(abilityContainerArray[i].name, abilityContainerArray[i]);
        }

        // Remove reference
        abilityContainerArray = null;
    }

    public Ability[] GetCurrentAbilities()
    {
        Ability[] currentAbilities = new Ability[8];

        string abilityName;

        Debug.Log("AbilityMapper GetCurrentAbilities ability 0 is " + mapper.GetAbilityNameForIndex(0));

        for (int i = 0; i < 8; i++)
        {
            // Get 1st ability
            abilityName = mapper.GetAbilityNameForIndex(i + 1); // i+1 because array is 0 indexed and mapper is 1 indexed

            Debug.Log("AbilityFactory GetCurrentAbilities ability " + (i + 1) + " is " + abilityName);

            // Spawn it and add it to the list
            currentAbilities[i] = CreateAbility(abilityName);
            Debug.Log("AbilityFactory GetCurrentAbilities after ability is created name is  " + currentAbilities[i].name);
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

    // Call this to load abilities from the Resource folder
    void LoadAbilities()
    {
        abilityContainerArray[0] = Resources.Load<ProjectileAbility>("FireballAbility");
        abilityContainerArray[1] = Resources.Load<ProjectileAbility>("FireStormAbility");
        abilityContainerArray[2] = Resources.Load<ProjectileAbility>("BlastAbility");
        abilityContainerArray[3] = Resources.Load<ProjectileAbility>("TraceAbility");
        abilityContainerArray[4] = Resources.Load<ProjectileAbility>("BubbleAbility");
        abilityContainerArray[5] = Resources.Load<ProjectileAbility>("WaterRainAbility");
        abilityContainerArray[6] = Resources.Load<ProjectileAbility>("ManaSphereAbility");
        abilityContainerArray[7] = Resources.Load<ProjectileAbility>("IceWallAbility");
        abilityContainerArray[8] = Resources.Load<ProjectileAbility>("SpikesAbility");
        abilityContainerArray[9] = Resources.Load<ProjectileAbility>("RootsAbility");
        abilityContainerArray[10] = Resources.Load<ProjectileAbility>("EarthquakeAbility");
        abilityContainerArray[11] = Resources.Load<ProjectileAbility>("DustDuskAbility");
        abilityContainerArray[12] = Resources.Load<ProjectileAbility>("PushAbility");
        abilityContainerArray[13] = Resources.Load<ProjectileAbility>("RushAbility");
        abilityContainerArray[14] = Resources.Load<ProjectileAbility>("BlinkAbility");
        abilityContainerArray[15] = Resources.Load<ProjectileAbility>("TornadoAbility");
    }
}
