using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AbilityDataCache : MonoBehaviour
{
    static Dictionary<string, AbilityData> dataMap;

    // We will use this to load the configuration for the speed of the projectiles
    //static Dictionary<string, float> projectileSpeedMap;
    static ProjectileSpeedConfig projectileSpeedConfig;

    [SerializeField] AbilityDataList loadedAbilityData;

    // Use this for initialization
    void Awake()
    {
        LoadAbilityData();
        LoadProjectileSpeeds();
    }

    // Will return us the AbilityData of a specific ability
    public static AbilityData GetDataForAbility(string abilityName)
    {
        Debug.Log("AbilityData GetDataForAbility name is " + abilityName);
        return dataMap[abilityName];
    }

    void LoadAbilityData()
    {
        dataMap = new Dictionary<string, AbilityData>();

        // Load data from file
        string dataString = FileHandler.ReadString("AbilityConfig");
        loadedAbilityData = JsonUtility.FromJson<AbilityDataList>(dataString);

        foreach (var abilityData in loadedAbilityData.dataList)
        {
            // Map them by the name
            dataMap.Add(abilityData.description.name, abilityData);
        }

        loadedAbilityData.dataList.Clear();
    }

    void LoadProjectileSpeeds()
    {
        string dataString = FileHandler.ReadString("ProjectileSpeedConfig");
        projectileSpeedConfig = JsonUtility.FromJson<ProjectileSpeedConfig>(dataString);
    }

    // Could do this with a dict but we have only 3 entries
    public static float GetProjectileSpeed(string name)
    {
        switch(name)
        {
            case "FireballProjectile":
                return projectileSpeedConfig.FireballSpeed;

            case "FireStormProjectile":
                return projectileSpeedConfig.FireStormSpeed;

            case "TornadoProjectile":
                return projectileSpeedConfig.TornadoSpeed;

            default:
                return 0f;
        }
    }
}
