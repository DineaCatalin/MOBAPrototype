using UnityEngine;
using System.Collections;

public abstract class Ability : MonoBehaviour
{
    // This name will be used to load the correct AbilityData for this Ability
    [SerializeField] new string name;

    // If true the ability will be cast instantly without the spell indicator step
    public bool isInstant;

    // The cooldown of the ability at the current moment
    float currentCooldown;

    // Cache the cooldown value of this ability
    float cooldown;
    //AbilityData abilityData;

    // Can we cast the ability or is it still on cooldown
    protected bool isCharging = false;

    // 
    protected int playerID;

    // This is the helper that will guide the player to cast the ability
    // Will be empty for empty abilities
    [SerializeField] protected GameObject spellIndicator;

    // Cache reference to the ability data, we will use this in the ability manager
    [SerializeField] protected AbilityData abilityData;

    public virtual void Load()
    {
        Debug.Log("Ability Load name is " + name);
        abilityData = AbilityDataCache.GetDataForAbility(name);

        //Debug.Log("Ability Base is loading ability with cooldown" + abilityData.stats.cooldown);
        cooldown = abilityData.stats.cooldown;
    }

    public void UpdateCooldown()
    {
        currentCooldown -= Time.deltaTime;
        //Debug.Log(abilityData.description.name + " Updating cooldown " + currentCooldown);

        // Reset cooldown
        if(currentCooldown <= 0)
        {
            currentCooldown = cooldown;
            isCharging = false;
        }
    }

    public void ResetCooldown()
    {
        currentCooldown = cooldown;
    }

    // Cast the ability, as this is the base class we will only set the isCharging flag
    public virtual bool Cast()
    {
        return true;
    }

    public void ResetCharging()
    {
        isCharging = true;
    }

    public bool IsCharging()
    {
        return isCharging;
    }

    public GameObject PrepareSpellIndicator()
    {
        return spellIndicator;
    }

    public int GetManaCost()
    {
        return abilityData.stats.manaCost;
    }

    // Trying to set the spell indicator back to the instantiated object from the abilitymanager
    // We send a template so that the abilitymanager will spawn it and then we retake the reference also here
    public void SetSpellIndicator(GameObject instantiatedSpellIndicator)
    {
        spellIndicator = instantiatedSpellIndicator;
    }

    public void SetPlayerID(int id)
    {
        playerID = id;

        SpellIndicator spellInd = GetComponent<SpellIndicator>();

        if (spellInd != null)
            spellInd.playerID = id;
    }
}
