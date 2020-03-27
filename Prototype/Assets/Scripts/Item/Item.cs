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
            //GameManager.Instance.PlayerItemPickup(itemData, player.GetID());

            // Send the item back to the pool if it belongs to the pool
            // Items with index smaller then 0 will be items spawned by abilities
            if (index >= 0)
                ItemPool.DeactivateItem(index);
            else
                Destroy(gameObject);
        }
    }
}
