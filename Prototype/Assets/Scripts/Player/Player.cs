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

    // Defines the stats of our player like health, mana, power, speed
    [SerializeField] PlayerData stats;

    // This will be activated by the bubble shield ability
    Shield shield;

    PlayerController controller;

    new Rigidbody2D rigidbody;

    [SerializeField] int id;

    void Awake()
    {
        // Assign id from gamemanager, do it over the network

        SetComponentIDs();

        // Load stats from config file
        stats = PlayerDataLoader.Load();
        controller = GetComponent<PlayerController>();
        shield = GetComponentInChildren<Shield>();
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // This function will be called when a player touches an item
    // The player will receive different stats or buffs depending on the item
    public void PickupItem(ItemData item)
    {
        if (item.health > 0)
            Heal(item.health);

        if(item.mana > 0)
            IncreaseMana(item.mana);

        if (item.powerMultiplier > 0)
        {
            AddBuff(Buff.power, item.powerMultiplier, item.duration);
        }

        if (item.speedMultiplier > 0)
        {
            AddBuff(Buff.speed, item.speedMultiplier, item.duration);
        }
    }

    //
    //  BUFF MANAGMENT
    //

    // 
    void AddBuff(Buff buff, float buffValue, float duration)
    {
        if(buff == Buff.power)
        {
            stats.power *= buffValue;
            // TODO: add buff to the buff list
            StartCoroutine(RemoveBuff(buff, buffValue, duration));
        }
        else if(buff == Buff.speed)
        {
            stats.speed *= buffValue;
            // TODO: add buff to the buff list
            StartCoroutine(RemoveBuff(buff, buffValue, duration));
        }

    }

    // This is called once 
    IEnumerator RemoveBuff(Buff buff, float buffValue, float duration)
    {
        yield return new WaitForSeconds(duration);

        if(buff == Buff.power)
        {
            stats.power /= buffValue;
            // TODO: add remove to the buff list
        }
        else if (buff == Buff.speed)
        {
            stats.speed /= buffValue;
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
        // Manage shield : if the shield is active and the damage delt is not enogh to destroy
        // the shield don't appy damage to the player
        if (shield.IsActive() && !shield.IsDamageFatal(damage))
            return;

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

    // Adds mana
    public void IncreaseMana(int mana)
    {
        // Don't heal more then the maxHP
        if (stats.mana + mana >= stats.maxMana)
            stats.mana = stats.maxMana;
        else
            stats.mana += mana;
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

        StartCoroutine(RemoveRoot(duration));
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

    public void ApplyShield(int armor)
    {
        shield.SetArmor(armor);
    }

    public PlayerData GetStats()
    {
        return stats;
    }

    // This effect will throw the player in a random direction
    public void Kockout(int force, int damage)
    {

        Debug.Log("Knocking out player");
        Damage(damage);

        float x = force * Mathf.Pow(-1, Random.Range(0,2));
        
        float y = force * Mathf.Pow(-1, Random.Range(0, 2));

        Vector2 pushForce = new Vector2(x, y);

        Debug.Log("Pushing with force " + pushForce);

        rigidbody.AddForce(pushForce, ForceMode2D.Impulse);
    }

    public void PullToLocation(Vector3 targetPosition, int force, int damage)
    {
        Damage(damage);

        Vector3 direction = targetPosition - transform.position;
        direction.Normalize();
        Debug.Log("Direction multiplying by " + force + " before is " + direction);
        direction *= force;
        Debug.Log("Direction after  is " + direction);

        rigidbody.AddForce(direction, ForceMode2D.Impulse);
    }

    void SetComponentIDs()
    {
        var children = GetComponentsInChildren<Transform>();
        foreach (var child in children)
        {
            Debug.Log("Child is " + child.name);
            child.name = child.name + id;
        }

    }

    public int GetID()
    {
        return id;
    }
}
