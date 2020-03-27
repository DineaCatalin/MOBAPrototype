using UnityEngine;
using System;
using Random = UnityEngine.Random;
using Photon.Pun;
using System.Collections.Generic;

public class ItemPool : MonoBehaviour
{
    // This is the size of the ItemPool,
    // Will be initialized through the editor or through a config file
    [SerializeField] int poolSize;

    // We will use this item to create all the other items by calling Instantiate(...)
    [SerializeField] Item templateItem;

    float newItemTimer = 15f;

    public Vector2 spawnPosition;

    // Minimum size is 4 as we have 4 item types
    const int minSize = 2;

    // Data container for the item pool
    [SerializeField] static Item[] items;

    // We will use this to keep track of our current position in the pull
    private int currentIndex;

    ItemDataList loadedItemData;

    ItemData[] itemDatas;

    PhotonView photonView;

    // Cache the template for the mana item so we can grab the data from outside
    //static ItemData manaItemTemplate;

    Coroutine spawnCoroutine;

    // Use this for initialization
    void Start()
    {
        // Cache photonView
        photonView = GetComponent<PhotonView>();

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

        spawnPosition = Vector2.zero;

        // TODO: Load items
        // For the moment we are gonna do random items
        for (int i = 0; i < poolSize; i++)
        {
            Item item = Instantiate(templateItem, transform);
            item.index = i;     // Set the index of out item so that we can easily grab it later
            item.SetAttributes(itemDatas[i % poolSize]);   //Set item data
            Debug.Log("ItemPool Start Getting color for name " + item.itemData.name);

            // Set color
            item.GetComponent<SpriteRenderer>().color = GetItemColor(item.itemData.name);

            items[i] = item;
            item.gameObject.SetActive(false);
        }

        // Clear item data array
        Array.Clear(itemDatas, 0, itemDatas.Length);

        EventManager.StartListening("ItemPickedUp", new System.Action(OnRoundStart));

        // Test - spawn random items once every 5 seconds
        //InvokeRepeating("SpawnItem", 1f, newItemTimer);
        Invoke("SpawnItem", 1f);
    }

    void OnRoundStart()
    {

    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            SpawnItem();
        }
    }

    // Spawns item in a random location
    void SpawnItem()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("SpawnItemRPC", RpcTarget.AllBuffered, spawnPosition.x, spawnPosition.y, 1);
            currentIndex++;
        }
    }

    [PunRPC]
    void SpawnItemRPC(float posX, float posY, int index)
    {
        SpawnItem(spawnPosition, index);
    }

    // Spawn an item in the specified position
    void SpawnItem(Vector3 position, int index)
    {
        Debug.Log("ItemPool SpawnItem currentIndex is " + index + " name is " + items[index].itemData.name);
        Item item = items[index];
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
            if (itemData.name == "HP Shpere" || itemData.name == "Power Sphere")
            {
                itemDatas[index] = itemData;
                index++;
            }
        }

        loadedItemData.itemList.Clear();
    }

    public static void DeactivateItem(int index)
    {
        items[index].gameObject.SetActive(false);
    }

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
