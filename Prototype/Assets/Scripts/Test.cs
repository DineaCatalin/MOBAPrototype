using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Test : MonoBehaviour
{
    //[SerializeField] AbilityDataList dataList;
    //[SerializeField] AbilityData testData;
    [SerializeField] PlayerData data;

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


        string dataString = FileHandler.ReadString("Player");
        Debug.Log(dataString);
        data = JsonUtility.FromJson<PlayerData>(dataString);

    }
}
