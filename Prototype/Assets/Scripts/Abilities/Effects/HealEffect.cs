using UnityEngine;
using System.Collections;

public class HealEffect : AbilityEffect
{
    public override void ApplyEffect(Player player, AbilityStats stats)
    {
        // Fire ball damage
        player.Heal(stats.hpValue);

        // Dot burn damage
        player.ApplyHeal(stats.duration, stats.dotValue);
    }
}
