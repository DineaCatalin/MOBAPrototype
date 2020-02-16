using UnityEngine;
using System.Collections;

public class KnockoutEffect : AbilityEffect
{
    public override void ApplyEffect(Player player, AbilityStats stats)
    {
        player.Kockout(stats.dotValue, stats.hpValue);
    }
}
