using UnityEngine;
using System.Collections;

public class DustDuskAbility : Ability
{
    public override void Cast()
    {
        base.Cast();

        EnvironmentManager.Instance.TriggerDustDusk(abilityData.stats.duration);
    }
}
