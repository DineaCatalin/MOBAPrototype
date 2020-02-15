using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AbilityDataCache : MonoBehaviour
{
    static Dictionary<string, AbilityData> dataMap;

    [SerializeField] AbilityDataList loadedAbilityData;

    // Use this for initialization
    void Awake()
    {
        dataMap = new Dictionary<string, AbilityData>();

        // Load data from file
        string dataString = FileHandler.ReadString("AbilityConfig");
        //Debug.Log(dataString);
        loadedAbilityData = JsonUtility.FromJson<AbilityDataList>(dataString);

        foreach (var abilityData in loadedAbilityData.dataList)
        {
            // Map them by the name
            dataMap.Add(abilityData.description.name, abilityData);
        }

        loadedAbilityData.dataList.Clear();
    }

    // Will return us the AbilityData of a specific ability
    public static AbilityData GetDataForAbility(string abilityName)
    {
        Debug.Log("AbilityData GetDataForAbility name is " + abilityName);
        return dataMap[abilityName];
    }
}
