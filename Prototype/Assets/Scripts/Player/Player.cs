using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;

public enum Buff
{
    speed,
    power
}

public class Player : MonoBehaviour
{
    public static int localPlayerID;
    public static int localTeamID;

    [SerializeField] int id;
    public int teamID;

    public TextMeshProUGUI nickName;

    // Tells us in which team our player is
    // For now we default it to 1
    public string teamName = "Team1";

    // Defines the stats of our player like health, mana, power, speed
    [SerializeField] PlayerData stats;

    [SerializeField] SpriteRenderer graphics;

    public InteractionManager interactionManager;
    public StateManager rushAreaManager;

    public Transform castOrigin;

    Rigidbody2D rigidBody;
    Collider2D playerCollider;

    // This will be activated by the bubble shield ability
    Shield shield;

    // UI of the player
    public HealthBar healthBar;
    public ManaBar manaBar;

    int healthRegen;
    int manaRegen;

    // This will tell us when a player is receiving damage over time
    // We will not regen the player while this is happening
    bool isRecevingDOT;

    // Will help us determine if the player is already slowed so that the slow effect doesn't
    // stack and slow the player way to much
    bool isPlayerSlowed;

    // Used to track if the player is being healed by Water Rain
    bool healEffectActive;

    // Used to store the coroutine that is called when the player enters the Area Limiter zone
    // and starts taking damage. This variable is used to stop the coroutine that damages the player
    // Without it we couldn't stop the coroutine as we woudn't have a reference to it
    Coroutine dotCoroutine;

    public float rushAreaSpeedBoost;

    [HideInInspector] public bool hasDoubleDamage;

    // Will be activated when player starts charging mana and will be stoped when the players
    // releases the mana charge key
    Coroutine manaChargeCoroutine;
    int manaCoroutineCallsPerSecond;
    int manaChargePerTick;

    float baseSpeed;

    bool isAlive;

    [HideInInspector] public bool isNetworkActive;

    void Awake()
    {
        id = GetComponentInParent<PhotonView>().ViewID;

        SetComponentIDs();
       
        // Load stats from config file
        stats = PlayerDataLoader.Load();
        interactionManager = GetComponentInParent<InteractionManager>();
        shield = GetComponentInChildren<Shield>();
        rushAreaManager = GetComponentInChildren<StateManager>();
        rigidBody = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();

        healEffectActive = false;
        isRecevingDOT = false;

        manaCoroutineCallsPerSecond = 5;
        manaChargePerTick = (int)(stats.manaChargePerSecond / manaCoroutineCallsPerSecond);

        baseSpeed = stats.speed;
   }

    private void Start()
    {
        SetLocalID();

        if (isNetworkActive)
            LocalPlayerReferences.Load(gameObject, this, transform, castOrigin, rushAreaManager.gameObject);

        // Set health and mana bar values
        healthBar.SetMaxHealth(stats.maxHealth);
        manaBar.SetMaxMana(stats.maxMana);

        // Load health regen and mana regen from config
        healthRegen = 1;
        manaRegen = 1;

        isAlive = true;

        InvokeRepeating("RegenerateStats", 1, 3);

        // Start player at the beginning of the round
        EventManager.StartListening("StartRound", new System.Action(OnRoundStart));
        EventManager.StartListening("ShieldDestroyed", new System.Action(DeactivateShield));
    }

    void SetLocalID()
    {
        if (isNetworkActive)
            localPlayerID = id;
    }

    void RegenerateStats()
    {
        if (isRecevingDOT)
            return;
        Debug.Log("Player RegenerateStats");
        Heal(healthRegen);
        //IncreaseMana(manaRegen);
    }

    public void StartManaCharge()
    {
        Debug.Log("Player StartManaCharge");
        manaChargeCoroutine = StartCoroutine(ChargeMana());
    }

    public void StopManaCharge()
    {
        Debug.Log("Player StopManaCharge");
        if(manaChargeCoroutine != null)
            StopCoroutine(manaChargeCoroutine);
    }

    IEnumerator ChargeMana()
    {
        while(true)
        {
            yield return new WaitForSeconds(1f / manaCoroutineCallsPerSecond);
            IncreaseMana(manaChargePerTick);
        }
    }

    public void PickUpItem(ItemData itemData)
    {
        Debug.Log("Player PickUp");

        switch (itemData.name)
        {
            case "HP Sphere":
                {
                    Debug.Log("Player PickUp picking health item");
                    Heal(itemData.health);
                    break;
                }
            case "Mana Sphere":
                {
                    IncreaseMana(itemData.mana);
                    break;
                }
            case "Power Sphere":
                {
                    HandleDoubleDamage(itemData.duration);
                    break;
                }
            case "Speed Sphere":
                {
                    PickUpSpeedItem(itemData.duration, itemData.speedMultiplier);
                    break;
                }
            default:
                return;
        }
    }

    public void PickUpSpeedItem(float duration, float value)
    {
        AddBuff(Buff.speed, duration, value);
    }

    void HandleDoubleDamage(float duration)
    {
        hasDoubleDamage = true;
        Debug.Log("Player " + id + " AddDoubleDamage");
        Invoke("RemoveDoubleDamage", duration);
    }

    void RemoveDoubleDamage()
    {
        Debug.Log("Player " + id + " RemoveDoubleDamage");
        hasDoubleDamage = false;
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

    public void DamageAndDOT(int initialDamage, int dotTicks, int dotDamage)
    {
        if (shield.IsActive())
        {
            Debug.Log("Player DamageAndDOT shield is active");
            Damage(initialDamage);
        }
        else
        {
            Debug.Log("Player DamageAndDOT shield is not active also applying DOT");
            Damage(initialDamage);
            ApplyDOT(dotTicks, dotDamage);
        }
    }

    // Will apply damage over time
    public void ApplyDOT(int numTicks, int damage)
    {
        if(gameObject.activeSelf)
        {
            isRecevingDOT = true;
            StartCoroutine(ApplyTickDamage(numTicks, damage));
        }
    }

    // This will be triggered by the AreaLimiter and will only be deactivated when
    // the player leaves the Area limiter 
    public void ApplyArenaLimiterDOT(int damage, float damageInterval)
    {
        Debug.Log("Player ApplyArenaLimiterDOT damage " + damage + " damageInterval " + damageInterval);
        dotCoroutine = StartCoroutine(ApplyDOTInfinite(damage, damageInterval));
    }

    public void DisableArenaLimiterDOT()
    {
        Debug.Log("Player " + id + " DisableArenaLimiterDOT");
        StopCoroutine(dotCoroutine);
    }

    IEnumerator ApplyDOTInfinite(int damage, float damageInterval)
    {
        while(true)
        {
            Damage(damage);
            yield return new WaitForSeconds(damageInterval);
        }
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
        if (shield.IsActive())
        {
            shield.Damage(ref damage);
            Debug.Log("Player " + id + " shield is active so no damage taken");
            return;
        }

        Debug.Log("Player " + id + " health before " + stats.health);
        stats.health -= damage;
        Debug.Log("Player " + id + " health after " + stats.health);
        healthBar.SetCurrentHealth(stats.health);

        Debug.Log("Player " + id + " isAlive " + isAlive);

        // Check if we are dead
        if (stats.health <= 0 && isAlive)
        {
            Debug.Log("Player " + id + " Die()");
            Die();
        }
    }

    void Die()
    {
        // We will just set the GO as inactive
        Debug.Log("Player " + id + " Die()");
        GameManager.Instance.KillNetworkedPlayer(this.id);
    }

    public void Deactivate()
    {
        nickName.gameObject.SetActive(false);
        healthBar.gameObject.SetActive(false);
        manaBar.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    // Will be used for synching the teleport mechanic over the network
    public void HandlePlayerDeath()
    {
        Debug.Log("Player HandlePlayerDeath");
        isAlive = false;

        Deactivate();

        // Handle slow, shield and rush area
        stats.speed = baseSpeed;
        rushAreaManager.Deactivate();
        shield.DeactivateNetworkedShield();

        StopManaCharge();
        RemoveDoubleDamage();

        PlayerController.isLocked = true;
    }

    private void Reset()
    {
        // Reset stats here so that player doesn't appear with 0 hp and mana
        // Stats
        stats.health = stats.maxHealth;
        stats.mana = stats.maxMana;

        // UI
        healthBar.SetCurrentHealth(stats.health);
        manaBar.SetCurrentMana(stats.mana);

        // Activate my player and set its start position
        // and then tell the other clients to activate my version of their player
        if (isNetworkActive)
        {
            transform.position = EnvironmentManager.Instance.GetPlayerSpawnPoint(teamID);
            Activate();
            GameManager.Instance.ActivateNonLocalPlayer(id);
            PlayerController.isLocked = false;
        }   
    }

    public void Activate()
    {
        Debug.Log("Player Activate");

        isAlive = true;

        gameObject.SetActive(true); 
        healthBar.gameObject.SetActive(true);
        manaBar.gameObject.SetActive(true);
        nickName.gameObject.SetActive(true);

        PlayerController.isRooted = false;
    }

    void OnRoundStart()
    {
        Debug.Log("Player OnRoundStart");
        Reset();
    }

    public bool IsAlive()
    {
        return isAlive;
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

        // DOT has finished 
        isRecevingDOT = false;
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
        //controller.isRooted = true;
        PlayerController.isRooted = true;

        if (gameObject.activeSelf)
            StartCoroutine(RemoveRoot(duration));
    }

    IEnumerator RemoveRoot(int duration)
    {
        yield return new WaitForSeconds((float)duration);
        //controller.isRooted = false;
        PlayerController.isRooted = false;
    }

    // Slow the player for duration seconds by slowValue
    public void Slow(int slowValue, float duration, bool stackEffect = false)
    {
        Debug.Log("Player Slow slowing by " + slowValue);

        if ((!isPlayerSlowed || stackEffect) && gameObject.activeSelf)
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

    void RushAreaSpeedBost(float time)
    {
        stats.speed += rushAreaSpeedBoost;
        Invoke("RemoveSpeedBoost", time);
    }

    void RemoveSpeedBoost()
    {
        stats.speed -= rushAreaSpeedBoost;
    }

    public void ActivateRushArea(float duration)
    {
        Debug.Log("Player ActivateRushArea activating rush area for player " + id);
        rushAreaManager.Activate(duration);

        if (isNetworkActive)
        {
            RushAreaSpeedBost(duration);
            Debug.Log("Player ActivateRushArea adding speed bost to local player " + id);
        }
    }

    // Set the shield only if the player is the 1 clicked
    public void ActivateShield(int armor)
    {
        shield.SetArmor(armor);

        if (isNetworkActive)
        {
            GameManager.Instance.ActivatePlayerShield(armor, id);
        }
    }

    public void DeactivateShield()
    {
        if (isNetworkActive)
            GameManager.Instance.DeactivatePlayerShield(id);
        
        shield.DeactivateLocalShield();
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

        this.teamID = teamID;
        teamName = "Team" + teamID;

        // We will also set the color of the sprite here temporarily
        if (teamID == 2)
        {
            graphics.color = Color.red;
        }

        if(isNetworkActive)
        {
            localTeamID = teamID;

            // Set position for the ist spawn
            transform.position = EnvironmentManager.Instance.GetPlayerSpawnPoint(teamID);

        }
    }
}
