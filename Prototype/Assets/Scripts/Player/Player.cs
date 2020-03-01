using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
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

    public PlayerController controller;
    public InteractionManager interactionManager;
    public StateManager rushAreaManager;

    Rigidbody2D rigidBody;

    // ID of the player used to identify abilities
    [SerializeField] int id;

    // UI of the player
    public HealthBar healthBar;
    public ManaBar manaBar;

    public PhotonView photonView;

    // Used to track if the player is being healed by Water Rain
    bool healEffectActive;

    // Will help us determine if the player is already slowed so that the slow effect doesn't
    // stack and slow the player way to much
    bool isPlayerSlowed;

    void Awake()
    {
        // TODO: Assign id from gamemanager, do it over the network

        id = GetComponentInParent<PhotonView>().ViewID;

        SetComponentIDs();

        // Load stats from config file
        stats = PlayerDataLoader.Load();
        controller = GetComponentInParent<PlayerController>();
        interactionManager = GetComponentInParent<InteractionManager>();
        shield = GetComponentInChildren<Shield>();
        rushAreaManager = GetComponentInChildren<StateManager>();
        rigidBody = GetComponent<Rigidbody2D>();

       healEffectActive = false;
    }

    private void Start()
    {
        // Set health and mana bar values
        healthBar.SetMaxHealth(stats.maxHealth);
        manaBar.SetMaxMana(stats.maxMana);
    }

    // Not the best way to do it but the items are simple in this prototype
    public void PickUpHPItem(int value)
    {
        Heal(value);
    }

    public void PickUpManaItem(int value)
    {
        IncreaseMana(value);
    }

    public void PickUpSpeedItem(float duration, float value)
    {
        AddBuff(Buff.speed, duration, value);
    }

    public void PickUpPowerItem(float duration, float value)
    {
        AddBuff(Buff.power, duration, value);
    }

    //
    //  BUFF MANAGMENT
    //

    // 
    void AddBuff(Buff buff, float duration, float buffValue)
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
        if(gameObject.activeSelf)
            StartCoroutine(ApplyTickDamage(numTicks, damage));
    }

    // Will heal over time
    public void HealOverTime(int numTicks, int heal)
    {
        StartCoroutine(ApplyTickHeal(numTicks, heal));
    }

    public void Damage(int damage)
    {
        // Manage shield : if the shield is active and the damage delt is not enogh to destroy
        // the shield don't appy damage to the player
        if (shield.IsActive() && !shield.IsDamageFatal(ref damage))
        {
            Debug.Log("Player " + id + " shield is active so no damage taken");
            return;
        }

        Debug.Log("Player " + id + " health before " + stats.health);
        stats.health -= damage;
        Debug.Log("Player " + id + " health after " + stats.health);
        healthBar.SetCurrentHealth(stats.health);

        // Check if we are dead
        if(stats.health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Update match statistics etc.

        // We will just set the GO as inactive
        Debug.Log(gameObject + " Die and respawn in " + GameManager.RESPAWN_COOLDOWN + " seconds");

        GameManager.Instance.KillAndRespawnPlayer(GameManager.RESPAWN_COOLDOWN, this.id);
    }

    // Will be used for synching the teleport mechanic over the network
    public void Deactivate()
    {
        healthBar.gameObject.SetActive(false);
        manaBar.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    public void Activate()
    {
        gameObject.SetActive(true);
        healthBar.gameObject.SetActive(true);
        manaBar.gameObject.SetActive(true);
    }

    // This will be called once the player has been respawned (reactivated) to reset stats
    public void Reset()
    {
        // Stats
        stats.health = stats.maxHealth;
        stats.mana = stats.maxMana;

        // UI1
        healthBar.SetCurrentHealth(stats.health);
        manaBar.SetCurrentMana(stats.mana);

        Activate();
    }

    public void Heal(int heal)
    {
        // Don't heal more then the maxHP
        if(stats.health + heal >= stats.maxHealth)
        {
            stats.health = stats.maxHealth;
            healthBar.SetCurrentHealth(stats.health);
        }
        else
        {
            stats.health += heal;
            healthBar.SetCurrentHealth(stats.health);
        } 
    }

    // Is the same as heal and healover time but it will not trigger if
    // the player is already healing from water rain heal
    public void WaterRainHeal(int initialHeal, int healTicks, int healTickValue)
    {
        if(!healEffectActive)
        {
            Debug.Log("Player WaterRainHeal applying heal");
            healEffectActive = true;

            Heal(initialHeal);
            HealOverTime(healTicks, healTickValue);
        }
    }

    // Adds mana
    public void IncreaseMana(int mana)
    {
        // Don't give more mana then the maxHP
        if (stats.mana + mana >= stats.maxMana)
        {
            stats.mana = stats.maxMana;
            manaBar.SetCurrentMana(stats.mana);
        }
        else
        {
            stats.mana += mana;
            manaBar.SetCurrentMana(stats.mana);
        } 
    }

    public void UseMana(int mana)
    {
        stats.mana -= mana;
        manaBar.SetCurrentMana(stats.mana);
    }

    public bool EnoughManaForAbility(int manaCost)
    {
        if (stats.mana - manaCost < 0)
            return false;

        return true;
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

        healEffectActive = false;
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
        Debug.Log("Player Slow slowing by " + slowValue);

        if(!isPlayerSlowed)
        {
            float speedDiff = Mathf.Abs(stats.speed - slowValue);

            // Descrease the players speed
            stats.speed -= slowValue;

            // If the slow is more then the speed set the speed to 0
            // so that we don't have negative movement speed
            if (stats.speed < 0)
            {
                // The slow value will be our speed value now because we will use
                // this to restore our speed after the slow effect is over
                slowValue = slowValue - (int)speedDiff;
                stats.speed = 0;
            }

            isPlayerSlowed = true;

            StartCoroutine(RemoveSlow(slowValue, duration));            
        }
    }

    IEnumerator RemoveSlow(int slowValue, float duration)
    {
        yield return new WaitForSeconds(duration);

        Debug.Log("Player RemoveSlow adding speed " + slowValue);

        // Reset speed to the original value
        stats.speed += slowValue;

        isPlayerSlowed = false;
    }

    // Set the shield only if the player is the 1 clicked
    public void ActivateShield(int armor)
    {
        shield.SetArmor(armor);
    }

    public void ActivateRushArea(float duration)
    {
        rushAreaManager.Activate(duration);
    }

    public PlayerData GetStats()
    {
        return stats;
    }

    public int GetHealth()
    {
        return stats.health;
    }

    public void SetHealth(int health)
    {
        stats.health = health;
        healthBar.SetCurrentHealth(health);
    }

    public int GetMana()
    {
        return stats.health;
    }

    public void SetMana(int mana)
    {
        stats.mana = mana;
        manaBar.SetCurrentMana(mana);
    }

    //
    // 
    //

    // This effect will throw the player in a random direction
    public void Knockout(int force, int damage)
    {
        Debug.Log("Knocking out player " + id);
        Damage(damage);

        float x = force * Mathf.Pow(-1, Random.Range(0,2));
        float y = force * Mathf.Pow(-1, Random.Range(0, 2));

        Vector2 pushForce = new Vector2(x, y);

        Debug.Log("Pushing with force " + pushForce + " player with id " + id);

        rigidBody.AddForce(pushForce, ForceMode2D.Impulse);
    }

    public void PullToLocation(Vector3 targetPosition, int force, int damage)
    {
        Damage(damage);

        Vector3 direction = targetPosition - transform.position;
        direction.Normalize();
        direction *= force;

        rigidBody.AddForce(direction, ForceMode2D.Impulse);
    }

    //
    // Manage ID and the passing of the ID to other components
    //

    void SetComponentIDs()
    {
        var children = GetComponentsInChildren<Transform>();
        foreach (var child in children)
        {
            //Debug.Log("Child is " + child.name);
            child.name = child.name + id;
        }

    }

    public int GetID()
    {
        return id;
    }

    // This will be called when the player is assigned to a team
    // The layer and tag of the player will be different depending on the team
    public void SetTeamSpecificData(int teamID)
    { 
        gameObject.tag = "Team" + teamID;
        gameObject.layer = LayerMask.NameToLayer("Team" + teamID + "Player");

        Debug.Log("Player SetTeamSpecificData tag " + gameObject.tag + " layer " + gameObject.layer);

        teamName = "Team" + teamID;

        // We will also set the color of the sprite here temporarily
        if (teamID == 2)
        {
            GetComponent<SpriteRenderer>().color = Color.red;
        }
    }
}
