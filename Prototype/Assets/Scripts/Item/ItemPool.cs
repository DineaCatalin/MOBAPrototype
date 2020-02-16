using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class ItemPool : MonoBehaviour
{
    // This is the size of the ItemPool,
    // Will be initialized through the editor or through a config file
    [SerializeField] int poolSize;

    // We will use this item to create all the other items by calling Instantiate(...)
    [SerializeField] Item templateItem;

    // Minimum size is 4 as we have 4 item types
    const int minSize = 4;

    // Data container for the item pool
    [SerializeField] static Item[] items;

    // We will use this to keep track of our current position in the pull
    private int currentIndex;

    ItemDataList loadedItemData;

    ItemData[] itemDatas;

    // Cache the template for the mana item so we can grab the data from outside
    //static ItemData manaItemTemplate;

    // Use this for initialization
    void Start()
    {
        // Call this to get the item data from the config file
        LoadItemData();

        // If there is no implementation in the config file, add the size from the inspector
        // If it is less then  the minimum one, take the minimum one
        if (poolSize > minSize)
        {
            items = new Item[poolSize];
        }     
        else
        {
            items = new Item[minSize];
            poolSize = minSize;
        }


        // TODO: Load items
        // For the moment we are gonna do random items
        for (int i = 0; i < poolSize; i++)
        {
            Item item = Instantiate(templateItem, transform);
            item.index = i;     // Set the index of out item so that we can easily grab it later
            item.SetAttributes(itemDatas[Random.Range(0, itemDatas.Length)]);   //Set item data
            Debug.Log("Getting color for name " + item.name);

            // Set color
            item.GetComponent<SpriteRenderer>().color = GetItemColor(item.itemData.name);

            items[i] = item;
            item.gameObject.SetActive(false);
        }

        // Clear item data array
        Array.Clear(itemDatas, 0, itemDatas.Length);

        // Test - spawn random items once every 5 seconds
        InvokeRepeating("SpawnItem", 5f, 5f);
    }


    public static void DeactivateItem(int index)
    {
        items[index].gameObject.SetActive(false);
    }


    // Spawn an item in the specified position
    void SpawnItem(Vector3 position)
    {
        Item item = items[currentIndex];
        item.transform.position = position;
        item.gameObject.SetActive(true);
        currentIndex = (currentIndex + 1) % poolSize;
    }

    // Spawns item in a random location
    void SpawnItem()
    {
        SpawnItem(Utils.GetRandomScreenPoint());
    }

    void LoadItemData()
    {
        string dataString = FileHandler.ReadString("ItemConfig");
        Debug.Log(dataString);
        loadedItemData = JsonUtility.FromJson<ItemDataList>(dataString);

        itemDatas = new ItemData[loadedItemData.itemList.Count];

        int index = 0;
        foreach (var itemData in loadedItemData.itemList)
        {
            itemDatas[index] = itemData;
            index++;

            // Cache mana item, we could do this for all items but not now
            //if (itemData.name == "Mana Sphere")
            //    manaItemTemplate = itemData;
        }

        loadedItemData.itemList.Clear();
    }

    //static ItemData GetManaItemTemplate()
    //{
    //    return manaItemTemplate;
    //}

    Color GetItemColor(string itemName)
    {
        switch(itemName)
        {
            case "HP Shpere":
            {
                    return Color.red;
            }
            case "Mana Sphere":
            {
                    return Color.blue;
            }
            case "Power Sphere":
            {
                    return Color.yellow;
            }
            case "Speed Sphere":
            {
                    return Color.green;
            }
            default:
            {
                    return Color.black;
            }
        }
    }

}
