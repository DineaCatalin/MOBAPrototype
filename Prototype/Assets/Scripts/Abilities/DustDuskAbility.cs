using UnityEngine;
using System.Collections;

public class DustDuskAbility : Ability
{
    int teamID;

    private void Start()
    {
        teamID = LocalPlayerReferences.player.teamID;
    }

    public override bool Cast()
    {
        EnvironmentManager.Instance.TriggerDustDusk(abilityData.stats.duration, teamID);
        return base.Cast();
    }
}
