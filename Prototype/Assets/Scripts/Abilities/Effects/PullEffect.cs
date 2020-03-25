using UnityEngine;
using System.Collections;

public class PullEffect : AbilityEffect
{
    public override void ApplyEffect(Player player, AbilityStats stats)
    {
        player.PullToLocation(transform.position, stats.dotValue, stats.hpValue);
    }
}
