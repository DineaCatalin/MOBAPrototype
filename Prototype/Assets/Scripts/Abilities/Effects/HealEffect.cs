using UnityEngine;
using System.Collections;

public class HealEffect : AbilityEffect
{
    public override void ApplyEffect(Player player, AbilityStats stats)
    {
        //GameManager.Instance.HealPlayerNoStacks(stats.hpValue, stats.duration, stats.dotValue, player.GetID());
        if(player.isNetworkActive)
            player.WaterRainHeal(stats.hpValue, stats.duration, stats.dotValue);
    }
}
