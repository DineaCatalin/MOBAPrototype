using UnityEngine;
using System.Collections;

public static class PlayerDataLoader
{
    public static PlayerData GetPlayerData()
    {
        PlayerData data = new PlayerData();
        string dataString = FileHandler.ReadString("PlayerConfig");
        data = JsonUtility.FromJson<PlayerData>(dataString);

        return data;
    }
}
