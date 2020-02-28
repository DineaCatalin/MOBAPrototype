﻿using UnityEngine;
using System.Collections;

public class HealEffect : AbilityEffect
{
    public override void ApplyEffect(Player player, AbilityStats stats)
    {
        GameManager.Instance.WaterRainHealPlayer(stats.hpValue, stats.duration, stats.dotValue, player.GetID()); ;
    }
}
