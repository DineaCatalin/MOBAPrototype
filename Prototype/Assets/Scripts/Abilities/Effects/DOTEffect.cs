using UnityEngine;
using System.Collections;

// This will be used by all Abilities that do damage on hit and then
// do DOT for a couple of seconds
public class DOTEffect : AbilityEffect
{
    public override void ApplyEffect(Player player, AbilityStats stats)
    {
        // Fire ball damage
        player.Damage(stats.hpDiff);

        // Dot burn damage
        player.ApplyDOT(stats.dotDuration, stats.dotDamage);
    }
}
