using UnityEngine;
using System.Collections;

public class RootEffect : AbilityEffect
{
    public override void ApplyEffect(Player player, AbilityStats stats)
    {
        Debug.Log("Applying root effect");
        player.Root(stats.duration);
    }
}
