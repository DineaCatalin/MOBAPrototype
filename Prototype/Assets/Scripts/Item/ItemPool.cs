using UnityEngine;
using System;
using Random = UnityEngine.Random;
using Photon.Pun;
using System.Collections.Generic;

public class ItemPool : MonoBehaviour
{
    // This is the size of the ItemPool,
    // Will be initialized through the editor or through a config file
    //[SerializeField] int poolSize;

    [SerializeField] Item[] items;

    // We will use this item to create all the other items by calling Instantiate(...)
    //[SerializeField] Item templateItem;

    public Vector2 spawnPosition;

    // Minimum size is 4 as we have 4 item types
    const int minSize = 2;

    // Data container for the item pool
    [SerializeField] static Item[] staticItems; //TODO: get rid of this

    ItemDataList loadedItemData;

    ItemData[] itemDatas;

    PhotonView photonView;

    // Use this for initialization
    void Start()
    {
        // Cache photonView
        photonView = GetComponent<PhotonView>();

        spawnPosition = Vector2.zero;

        staticItems = new Item[items.Length];

        for (int i = 0; i < staticItems.Length; i++)
        {
            Item item = Instantiate(items[i], transform);
            item.index = i;
            item.name = items[i].name;
            item.gameObject.SetActive(false);
            staticItems[i] = item;
        }

        items = null;

        // Call this to get the item data from the config file
        //LoadItemData();

        // If there is no implementation in the config file, add the size from the inspector
        // If it is less then  the minimum one, take the minimum one
        //if (poolSize > minSize)
        //{
        //    items = new Item[poolSize];
        //}     
        //else
        //{
        //    items = new Item[minSize];
        //    poolSize = minSize;
        //}



        // TODO: Load items
        // For the moment we are gonna do random items
        //for (int i = 0; i < poolSize; i++)
        //{
        //    Item item = Instantiate(templateItem, transform);
        //    item.index = i;     // Set the index of out item so that we can easily grab it later
        //    item.SetAttributes(itemDatas[i % poolSize]);   //Set item data
        //    Debug.Log("ItemPool Start Getting color for name " + item.itemData.name);

        //    // Set color
        //    item.GetComponent<SpriteRenderer>().color = GetItemColor(item.itemData.name);

        //    items[i] = item;
        //    item.gameObject.SetActive(false);
        //}

        // Clear item data array
        //Array.Clear(itemDatas, 0, itemDatas.Length);


        EventManager.StartListening(GameEvent.SpawnItem, new System.Action(SpawnItem));
        EventManager.StartListening(GameEvent.PlanetStateAdvance, new System.Action(DisableItems));
        EventManager.StartListening(GameEvent.EndRound, new System.Action(DisableItems));
    }

    // Spawns item in a random location
    void SpawnItem()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            int itemIndex = Random.Range(0, staticItems.Length);
            photonView.RPC("SpawnItemRPC", RpcTarget.AllBuffered, spawnPosition.x, spawnPosition.y, itemIndex);
        }
    }

    [PunRPC]
    void SpawnItemRPC(float posX, float posY, int index)
    {
        SpawnItem(spawnPosition, index);
    }

    // Spawn an item in the specified position
    // TODO this should not be static
    void SpawnItem(Vector3 position, int index)
    {
        Debug.Log("ItemPool SpawnItem currentIndex is " + index + " name is " + staticItems[index].itemData.name);
        Item item = staticItems[index];
        item.transform.position = position;
        item.gameObject.SetActive(true);
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
            // Take only heal and power (double damage) for now
            if (itemData.name == "HP Sphere" || itemData.name == "Power Sphere")
            {
                itemDatas[index] = itemData;
                index++;
            }
        }

        loadedItemData.itemList.Clear();
    }

    // TODO this should not be static
    public static void DeactivateItem(int index)
    {
        staticItems[index].gameObject.SetActive(false);
    }

    void DisableItems()
    {
        for (int i = 0; i < staticItems.Length; i++)
        {
            staticItems[i].gameObject.SetActive(false);
        }
    }

    Color GetItemColor(string itemName)
    {
        switch(itemName)
        {
            case "HP Sphere":
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
