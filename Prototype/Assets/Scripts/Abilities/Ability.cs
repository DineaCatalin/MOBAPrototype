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
    public bool isCharging;

    // Cache reference to the ability data, we will use this in the ability manager
    protected AbilityData abilityData;

    protected void Start()
    {
        abilityData = AbilityDataCache.GetDataForAbility(name);
        cooldown = abilityData.stats.cooldown;
    }

    public void UpdateCooldown(float time)
    {
        currentCooldown -= time;

        // Reset cooldown
        if(currentCooldown <= 0)
        {
            currentCooldown = 0;
            isCharging = false;
        }
    }

    public void ResetCooldown()
    {
        currentCooldown = cooldown;
    }

    // Cast the ability, as this is the base class we will only set the isCharging flag
    public virtual void Cast()
    {
        isCharging = true;
    }
}
