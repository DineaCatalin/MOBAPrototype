using UnityEngine;
using System.Collections;

public static class PlayerDataLoader
{
    public static PlayerData Load()
    {
        PlayerData data = new PlayerData();

        string dataString = FileHandler.ReadString("PlayerConfig");
        //            Debug.Log(dataString);
        data = JsonUtility.FromJson<PlayerData>(dataString);

        return data;
    }
    
}
