using UnityEngine;

public class AbilityMapper
{
    static AbilityMapperData data;

    public AbilityMapper()
    {
        if(data == null)
        {
            LoadData();
            Debug.Log("AbilityMapper loading data from file");
        }
        
    }

    public static void SetData(AbilityMapperData mapperData)
    {
        data = mapperData;
    }

    // This has to be reworked but due to the fact this is a prototype we'll leave it like this
    public string GetAbilityNameForIndex(int abilityIndex)
    {
        Debug.Log("GetAbilityNameForIndex getting ability for index " + abilityIndex);

        switch (abilityIndex)
        {
            case 1:
                return data.ability1;
            case 2:
                return data.ability2;
            case 3:
                return data.ability3;
            case 4:
                return data.ability4;
            case 5:
                return data.ability5;
            case 6:
                return data.ability6;
            case 7:
                return data.ability7;
            case 8:
                return data.ability8;
            default:
                return null;
        }
    }

    void LoadData()
    {
        // TODO: Add the SelectedAbilityConfig from the draft screen instead

        string dataString = FileHandler.ReadString("SelectedAbilitiesConfig");
        Debug.Log(dataString);
        data = JsonUtility.FromJson<AbilityMapperData>(dataString);
        Debug.Log("AbilityMapper 1st " + data.ability1);
        Debug.Log("AbilityMapper 2nd " + data.ability2);
        Debug.Log("AbilityMapper 3rd " + data.ability3);
        Debug.Log("AbilityMapper 4th " + data.ability4);
        Debug.Log("AbilityMapper 5th " + data.ability5);
        Debug.Log("AbilityMapper 6th " + data.ability6);
        Debug.Log("AbilityMapper 7th " + data.ability7);
        Debug.Log("AbilityMapper 8th " + data.ability8);
    }
}

// We will use this to map which 8 abilities from the 16 the player will have
// This will let us define this in a config file
[System.Serializable]
public class AbilityMapperData
{
    public string ability1;
    public string ability2;
    public string ability3;
    public string ability4;
    public string ability5;
    public string ability6;
    public string ability7;
    public string ability8;
}
