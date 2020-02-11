using UnityEngine;
using System.Collections;

// Abilities that will slow other players will implement this effect
public class SlowEffect : AbilityEffect
{
    public override void ApplyEffect(Player player, AbilityStats stats)
    {
        player.Slow(stats.duration, stats.dotValue);
    }
}
