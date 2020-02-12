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

    // Struct that defines the stats of our player like health, mana, power, speed
    [SerializeField]PlayerData stats;

    PlayerController controller;

    // Start is called before the first frame update
    void Start()
    {

        controller = GetComponent<PlayerController>();
        // TODO: Load stats from config file-

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

    // Will apply damage over time
    public void ApplyDOT(int numTicks, int damage)
    {
        StartCoroutine(ApplyTickDamage(numTicks, damage));
    }

    // Will heal over time
    public void ApplyHeal(int numTicks, int heal)
    {
        StartCoroutine(ApplyTickHeal(numTicks, heal));
    }

    public void Damage(int damage)
    {
        stats.health -= damage;

        // Check if we are dead
        if(stats.health <= 0)
        {
            //Die
        }
    }

    public void Heal(int heal)
    {
        // Don't heal more then the maxHP
        if(stats.health + heal >= stats.maxHealth)
            stats.health = stats.maxHealth;
        else
            stats.health += heal;
    }

    IEnumerator ApplyTickDamage(int numTicks, int damage)
    {
        for (int i = 0; i < numTicks; i++)
        {
            // Wait for 1 second before applying the 1st tick
            yield return new WaitForSeconds(1f);
            Damage(damage);
        }
    }

    IEnumerator ApplyTickHeal(int numTicks, int heal)
    {
        for (int i = 0; i < numTicks; i++)
        {
            // Wait for 1 second before applying the 1st tick
            yield return new WaitForSeconds(1f);
            Heal(heal);
        }
    }


    // Methods for appying root to the player
    // Root means that the player can't move
    public void Root(int duration)
    {
        controller.isRooted = true;
    }

    IEnumerator RemoveRoot(int duration)
    {
        yield return new WaitForSeconds((float)duration);
        controller.isRooted = false;
    }


    // Methods for slowing the player

    // Slow the player for duration seconds by slowValue
    public void Slow(int slowValue, float duration)
    {
        // Descrease the players speed
        stats.speed -= slowValue;

        // If the slow is more then the speed set the speed to 0
        // so that we don't have negative movement speed
        if (stats.speed < 0)
        {
            // The slow value will be our speed value now because we will use
            // this to restore our speed after the slow effect is over
            slowValue = (int)stats.speed;
            stats.speed = 0;
        }

        StartCoroutine(RemoveSlow(slowValue, duration));
    }

    IEnumerator RemoveSlow(int slowValue, float duration)
    {
        yield return new WaitForSeconds(duration);

        // Reset speed to the original value
        stats.speed += slowValue;
    }

    public void ApplyShield(int hp, int duration)
    {
        
    }

    void LoadPlayerData()
    {
        string dataString = FileHandler.ReadString("Player");
        Debug.Log(dataString);
        stats = JsonUtility.FromJson<PlayerData>(dataString);
    }
}
