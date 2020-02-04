using UnityEngine;
using System.Collections;

public class ItemPool : MonoBehaviour
{
    // This is the size of the ItemPool,
    // Will be initialized through the editor or through a config file
    [SerializeField] int poolSize;

    [SerializeField] Item templateItem;

    // Minimum size is 4 as we have 4 item types
    const int minSize = 4;

    // Data container for the item pool
    [SerializeField] static Item[] items;

    // We will use this to keep track of our current position in the pull
    private int currentIndex;

    // Use this for initialization
    void Start()
    {
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
            items[i] = item;
            item.gameObject.SetActive(false);
        }

        // Test - spawn random items once every 5 seconds
        InvokeRepeating("SpawnItem", 0f, 5f);
    }

    static void DeactivateItem(int index)
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

}
