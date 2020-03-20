﻿using UnityEngine;
using System.Collections;

// This will be used by all Abilities that do damage on hit and then
// do DOT for a couple of seconds
public class DOTEffect : AbilityEffect
{
    public override void ApplyEffect(Player player, AbilityStats stats)
    {
        player.DamageAndDOT(stats.hpValue, stats.duration, stats.dotValue);
        //GameManager.Instance.DamagePlayerWithDOT(stats.hpValue, stats.dotValue, stats.duration, player.GetID());
    }
}