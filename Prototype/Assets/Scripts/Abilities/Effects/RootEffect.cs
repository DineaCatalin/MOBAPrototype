using UnityEngine;
using System.Collections;

public class RootEffect : AbilityEffect
{
    public override void ApplyEffect(Player player, AbilityStats stats)
    {
        player.Root(stats.duration);
    }
}
