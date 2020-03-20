using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AbilityDataCache : MonoBehaviour
{
    static Dictionary<string, AbilityData> dataMap;

    // We will use this to load the configuration for the speed of the projectiles
    //static Dictionary<string, float> projectileSpeedMap;
    static ProjectileSpeedConfig projectileSpeedConfig;

    static AbilityCastRangeConfig castRangeConfig;

    [SerializeField] AbilityDataList loadedAbilityData;

    [SerializeField] Sprite spellIndicatorOutOfRangeSprite;

    static Sprite spellIndicatorOutOfRange;

    // Use this for initialization
    void Awake()
    {
        LoadAbilityData();
        LoadProjectileSpeeds();
        LoadAbilityCastRange();

        spellIndicatorOutOfRange = spellIndicatorOutOfRangeSprite;
    }

    // Will return us the AbilityData of a specific ability
    public static AbilityData GetDataForAbility(string abilityName)
    {
        Debug.Log("AbilityDataCache GetDataForAbility name is " + abilityName);
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
            Debug.Log("AbilityDataCache LoadAbilityData " + abilityData.description.name);
            dataMap.Add(abilityData.description.name, abilityData);
        }

        loadedAbilityData.dataList.Clear();
    }

    void LoadProjectileSpeeds()
    {
        string dataString = FileHandler.ReadString("ProjectileSpeedConfig");
        projectileSpeedConfig = JsonUtility.FromJson<ProjectileSpeedConfig>(dataString);
    }

    void LoadAbilityCastRange()
    {
        string dataString = FileHandler.ReadString("CastRangeConfig");
        Debug.Log("AbilityDataCache LoadAbilityCastRange " + dataString);
        castRangeConfig = JsonUtility.FromJson<AbilityCastRangeConfig>(dataString);
    }

    // Could do this with a dict but we have only 3 entries
    public static float GetProjectileSpeed(string name)
    {
        switch(name)
        {
            case "FireballProjectile":
                return projectileSpeedConfig.FireballSpeed;

            case "IceballProjectile":
                return projectileSpeedConfig.IceballSpeed;

            case "LightningballProjectile":
                return projectileSpeedConfig.LightningballSpeed;

            case "FireStormProjectile":
                return projectileSpeedConfig.FireStormSpeed;

            case "TornadoProjectile":
                return projectileSpeedConfig.TornadoSpeed;

            default:
                return 0f;
        }
    }

    public static float GetAbilityCastRange(string name)
    {
        switch (name)
        {
            case "Blink":
                return castRangeConfig.BlinkCastRange;

            case "IceWall":
                return castRangeConfig.IceWallCastRange;

            case "Trace":
                return castRangeConfig.TraceCastRange;

            case "Roots":
                return castRangeConfig.RootsCastRange;

            case "Spikes":
                return castRangeConfig.SpikesCastRange;

            case "ManaSphere":
            case "Mana Sphere":
                return castRangeConfig.ManaSphereCastRange;

            case "WaterRain":
                return castRangeConfig.WaterRainCastRange;

            case "Earthquake":
                return castRangeConfig.EarthquakeCastRange;

            case "Bubble":
                return castRangeConfig.BubbleCastRange;

            default:
            {
                Debug.Log("AbilityDataCache GetAbilityCastRange default from name " + name);
                return 0f;
            }
        }
    }

    public static Sprite GetSpellIndicatorOutOfRangeSprite()
    {
        return spellIndicatorOutOfRange;
    }
}
