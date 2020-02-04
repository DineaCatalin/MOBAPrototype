using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Items stats will be loaded from a config file
// Stats will be given to a specific player on contact
public class Item : MonoBehaviour
{
    [SerializeField] float duration;
    [SerializeField] int health;
    [SerializeField] int mana;
    [SerializeField] int powerMultiplier;
    [SerializeField] int speedMultiplier;

    ItemData itemData;

    // This is the index in the item pool,
    // we will use this index to activate/deactivate the item when needed
    public int index;

    // Use this to temporarily set the values, the actual values will be set from a config file later
    private void Start()
    {
        itemData.duration = duration;
        itemData.health = health;
        itemData.mana = mana;
        itemData.powerMultiplier = powerMultiplier;
        itemData.speedMultiplier = speedMultiplier;
    }

    public void SetAttributes(float duration, int health, int mana, int powerMultiplier, int speedMultiplier)
    {
        itemData.duration = duration;
        itemData.health = health;
        itemData.mana = mana;
        itemData.powerMultiplier = powerMultiplier;
        itemData.speedMultiplier = speedMultiplier;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // We hit a player
        if(collision.tag == "Player")
        {
            Debug.Log("Player has collided with item");

            Player player = collision.GetComponent<Player>();
            player.PickupItem(itemData);

            // TODO: send the item back to the pool
            // For the moment I am going to distroy it
            Destroy(this.gameObject);
        }
    }
}
