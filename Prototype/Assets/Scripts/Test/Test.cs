using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Test : MonoBehaviour
{
    //[SerializeField] AbilityDataList dataList;
    //[SerializeField] AbilityData testData;
    //[SerializeField] PlayerData data;
    //[SerializeField] AbilityMapperData data;
    //[SerializeField] ItemDataList itemDataList;
    [SerializeField] List<string> list;

    private void Start()
    {
        // Create json
        //dataList = new AbiliyDataList
        //{
        //    dataList = new List<AbilityData>()
        //};

        //for (int i = 0; i < 5; i++)
        //{
        //    AbilityData data = new AbilityData
        //    {
        //        stats = new AbilityStats
        //        {
        //            cooldown = 1,
        //            manaCost = 1,
        //            dotValue = 1,
        //            duration = 1,
        //            hpValue = 1
        //        },

        //        description = new AbilityDescription
        //        {
        //            name = "Test",
        //            casterTeamName = "Team1"
        //        }
        //    };

        //    dataList.dataList.Add(data);  
        //}

        //string jsonString = JsonUtility.ToJson(dataList);
        //Debug.Log(jsonString);

        //testData = AbilityDataCache.GetDataForAbility("Test1");

        //string jsonString = JsonUtility.ToJson(data);
        //Debug.Log(jsonString);


        //string dataString = FileHandler.ReadString("Player");
        //Debug.Log(dataString);
        //data = JsonUtility.FromJson<PlayerData>(dataString);


        //string jsonString = JsonUtility.ToJson(data);
        //Debug.Log(jsonString);
        //FileHandler.WriteString(jsonString, "SelectedAbilitiesConfig");

        string jsonString = JsonUtility.ToJson(list);
        Debug.Log(jsonString);

        List<string> l = new List<string>() { "AAA", "BBB" };
        string jsonString2 = JsonUtility.ToJson(l);
        Debug.Log(jsonString2);

    }
}