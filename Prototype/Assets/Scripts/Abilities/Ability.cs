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

    // Can we cast the ability or is it still on cooldown
    protected bool isCharging = true;

    protected int playerID;

    protected AbilityUI abilityUI;

    // This is the helper that will guide the player to cast the ability
    // Will be empty for empty abilities
    [SerializeField] protected GameObject spellIndicator;

    // Cache reference to the ability data, we will use this in the ability manager
    [SerializeField] protected AbilityData abilityData;

    public virtual void Load()
    {
        abilityData = AbilityDataCache.GetDataForAbility(name);

        //Debug.Log("Ability Base is loading ability with cooldown" + abilityData.stats.cooldown);
        cooldown = abilityData.stats.cooldown;
    }

    public void SetUI(AbilityUI ui)
    {
        abilityUI = ui;
        abilityUI.Load(abilityData);
    }

    public void UpdateCooldown()
    {
        if(isCharging)
        {
            currentCooldown -= Time.deltaTime;

            // Reset cooldown
            if (currentCooldown <= 0)
            {
                currentCooldown = cooldown;
                isCharging = false;
                abilityUI.StopCooldown();
            }
            else
            {
                abilityUI.UpdateCooldown(currentCooldown);
            }
        }
    }

    // Cast the ability, as this is the base class we will only set the isCharging flag
    public virtual bool Cast()
    {
        ResetCharging();
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

        SpellIndicator spellIndicatorComp = spellIndicator.GetComponent<SpellIndicator>();

        if (spellIndicatorComp != null)
            spellIndicatorComp.playerID = id;
    }
}
