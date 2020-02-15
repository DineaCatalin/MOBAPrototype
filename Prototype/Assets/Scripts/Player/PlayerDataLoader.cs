using UnityEngine;
using System.Collections;

public static class PlayerDataLoader
{
    static PlayerData data;

    public static PlayerData Load()
    {
        if(data == null)
        {
            string dataString = FileHandler.ReadString("PlayerConfig");
//            Debug.Log(dataString);
            data = JsonUtility.FromJson<PlayerData>(dataString);
        }

        return data;
    }
    
}
