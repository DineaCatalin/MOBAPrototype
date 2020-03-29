using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealAbility : Ability
{
    Player player;

    // Start is called before the first frame update
    void Start()
    {
        player = LocalPlayerReferences.player;
    }

    public override bool Cast()
    {
        player.Heal(abilityData.stats.hpValue);
        return base.Cast();
    }
}
