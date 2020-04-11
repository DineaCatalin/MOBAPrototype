using UnityEngine;
using System.Collections;

public class PullEffect : AbilityEffect
{
    public override void ApplyEffect(Player player, AbilityStats stats)
    {
        base.ApplyVisualEffect(player, visualEffect, stats);

        if (player.isNetworkActive)
        {
            player.PullToLocation(transform.position, stats.dotValue, stats.hpValue);
            base.ApplyLocalVisualEffects(player, visualEffect);
        }
    }
}
