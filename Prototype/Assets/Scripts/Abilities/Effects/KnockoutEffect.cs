using UnityEngine;
using System.Collections;

public class KnockoutEffect : AbilityEffect
{
    public override void ApplyEffect(Player player, AbilityStats stats)
    {
        base.ApplyVisualEffect(player, visualEffect, stats);

        if(player.isNetworkActive)
        {
            player.Knockout(stats.dotValue, stats.hpValue);
            base.ApplyLocalVisualEffects(player, visualEffect);
        }
            
    }
}
