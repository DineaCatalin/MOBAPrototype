using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Buff
{
    speed,
    power
}

public class Player : MonoBehaviour
{
    // Tells us in which team our player is
    // For now we default it to 1
    public string teamName = "Team1";

    // Struct that defines the stat of our player
    PlayerStats stats;

    // Start is called before the first frame update
    void Start()
    {
        stats = GetComponent<PlayerStats>();
    }

    // Update is called once per frame
    void Update()
    {
         
    }

    // This function will be called when a player touches an item
    // The player will receive different stats or buffs depending on the item
    public void PickupItem(ItemData item)
    {

        Debug.Log("PickupItem Health" + item.health);
        if (item.health > 0)
             stats.health += item.health;

        if(item.mana > 0)
            stats.mana += item.mana;

        if (item.powerMultiplier > 0)
        {
            Debug.Log("Before update : Power is " + stats.power);
            Debug.Log("After update : Power is " + stats.power);
            AddBuff(Buff.power, item.powerMultiplier, item.duration);
        }

        if (item.speedMultiplier > 0)
        {
            Debug.Log("Before update : Power is " + stats.power);
            AddBuff(Buff.speed, item.speedMultiplier, item.duration);
        }
    }

    //
    //  BUFF MANAGMENT
    //

    // 
    void AddBuff(Buff buff, int buffValue, float duration)
    {
        if(buff == Buff.power)
        {
            stats.power += buffValue;
            // TODO: add buff to the buff list
            StartCoroutine(RemoveBuff(buff, buffValue, duration));
        }
        else if(buff == Buff.speed)
        {
            stats.speed += buffValue;
            // TODO: add buff to the buff list
            StartCoroutine(RemoveBuff(buff, buffValue, duration));
        }

    }

    // This is called once 
    IEnumerator RemoveBuff(Buff buff, int buffValue, float duration)
    {
        yield return new WaitForSeconds(duration);

        Debug.Log("Starting buff removal");

        if(buff == Buff.power)
        {
            stats.power -= buffValue;
            Debug.Log("Removing power");
            // TODO: add remove to the buff list
        }
        else if (buff == Buff.speed)
        {
            stats.speed -= buffValue;
            Debug.Log("Removing speed");
            // TODO: add remove to the buff list
        }
    }

    // Will handle the effect the ability has on our player
    public void ApplyAbilityEffect(AbilityStats abilityStats)
    {
        
    }
  
}
