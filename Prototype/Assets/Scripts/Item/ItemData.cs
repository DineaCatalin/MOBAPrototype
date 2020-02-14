using System.Collections.Generic;

[System.Serializable]
public struct ItemData
{
    public string name;
    public float duration;
    public int health;
    public int mana;
    public float powerMultiplier;
    public float speedMultiplier;
}

[System.Serializable]
public class ItemDataList
{
    public List<ItemData> itemList;
}
