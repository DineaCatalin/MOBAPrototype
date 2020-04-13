using UnityEngine;
using System.Collections;

public class KnockoutEffect : AbilityEffect
{
    public override void ApplyEffect(Player player, AbilityStats stats, int casterID)
    {
        base.ApplyVisualEffect(player, visualEffect, stats);

        if(player.isNetworkActive)
        {
            player.Knockout(stats.dotValue, stats.hpValue, casterID);
            base.ApplyLocalVisualEffects(player, visualEffect);
        }
            
    }
}
