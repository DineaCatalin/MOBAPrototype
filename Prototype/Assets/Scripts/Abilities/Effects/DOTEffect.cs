using UnityEngine;
using System.Collections;

// This will be used by all Abilities that do damage on hit and then
// do DOT for a couple of seconds
public class DOTEffect : AbilityEffect
{
    private void Start()
    {
        visualEffect = PlayerEffect.DOT;
    }

    public override void ApplyEffect(Player player, AbilityStats stats, int casterID)
    {
        Debug.Log("DOTEffect ApplyEffect");

        base.ApplyVisualEffect(player, visualEffect, stats);

        if (player.isNetworkActive)
        {
            player.DamageAndDOT(stats.hpValue, stats.duration, stats.dotValue, casterID);
            base.ApplyLocalVisualEffects(player, visualEffect);
        }
            
    }
}