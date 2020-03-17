using UnityEngine;
using System.Collections;

public class DustDuskAbility : Ability
{
    public override bool Cast()
    {
        base.Cast();

        EnvironmentManager.Instance.TriggerDustDusk(abilityData.stats.duration);

        abilityUI.ActivateCooldown();

        return true;
    }
}
