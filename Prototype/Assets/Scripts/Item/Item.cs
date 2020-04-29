using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Items stats will be loaded from a config file
// Stats will be given to a specific player on contact
public class Item : MonoBehaviour
{
    public ItemData itemData;

    // This is the index in the item pool,
    // we will use this index to activate/deactivate the item when needed
    public int index;

    private void Start()
    {
        // TODO: Construct a cache to load all itemData and then use that to get the itemData for this item
        string dataString = FileHandler.ReadString("ItemConfig");
        Debug.Log(dataString);
        ItemDataList itemDataList = JsonUtility.FromJson<ItemDataList>(dataString);

        foreach (ItemData item in itemDataList.itemList)
        {
            if(item.name.Equals(name))
            {
                Debug.Log("Item Start setting itemData " + name);
                itemData = item;
            }
        }
    }

    public void SetAttributes(ItemData data)
    {
        itemData = data;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // We hit a player
        if(collision.tag == "Team1" || collision.tag == "Team2")
        {
            Debug.Log("Player has collided with item : power " + itemData.powerMultiplier + " speed " + itemData.speedMultiplier);

            Player player = collision.GetComponent<Player>();
            player.PickUpItem(itemData);

            // Send the item back to the pool if it belongs to the pool
            // Items with index smaller then 0 will be items spawned by abilities
            if (index >= 0)
                ItemPool.DeactivateItem(index);
            else
                Destroy(gameObject);

            EventManager.TriggerEvent(GameEvent.ItemPickedUp);
        }
    }
}
