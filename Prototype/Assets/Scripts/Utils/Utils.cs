using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;

public static class Utils
{
    public static Vector2 GetRandomScreenPoint()
    {
        float spawnY = Random.Range
                (Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).y, Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height)).y);
        float spawnX = Random.Range
            (Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).x, Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x);

        return new Vector2(spawnX, spawnY);
    }

    public static Vector3 GetMousePosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    public static string RemoveCloneFromName(string name)
    {
        return Regex.Replace(name, @"(Clone)", "");
    }

    // Will switch from from Team1Ability to Team2Ability or vice versa
    public static string SwitchPlayerLayerName(string layerName)
    {
        if(layerName.Contains("1"))
        {
            layerName = layerName.Replace("1", "2");
            return layerName;
        }
        else if(layerName.Contains("2"))
        {
            layerName = layerName.Replace("2", "1");
            return layerName;
        }

        // For some reason this func is called on a string that is not a player layer name
        return layerName;
    }
}
