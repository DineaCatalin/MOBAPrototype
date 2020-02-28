using UnityEngine;
using System.Collections;

public class HealEffect : AbilityEffect
{
    public override void ApplyEffect(Player player, AbilityStats stats)
    {
        // First big heal
        player.Heal(stats.hpValue);

        // Some heal over time
        player.HealOverTime(stats.duration, stats.dotValue);
    }
}
