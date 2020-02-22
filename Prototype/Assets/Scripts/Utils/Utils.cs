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
        return Camera.main.WorldToScreenPoint(Input.mousePosition);
    }

    public static string RemoveCloneFromName(string name)
    {
        return Regex.Replace(name, @"(Clone)", "");
    }
}
