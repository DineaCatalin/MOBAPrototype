using UnityEngine;
using System.Collections;

public class RootEffect : AbilityEffect
{
    public override void ApplyEffect(Player player, AbilityStats stats)
    {
        Debug.Log("Applying root effect");
        if (stats == null)
            Debug.Log("RootEffect stats are null wtf?");
        if(player.interactionManager == null)
            Debug.Log("RootEffect interactionManager is null wtf?");

        if (player.isNetworkActive)
            player.interactionManager.RootPlayer(stats.duration);
    }
}
