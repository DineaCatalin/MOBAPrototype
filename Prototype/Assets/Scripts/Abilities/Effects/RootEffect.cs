using UnityEngine;

public class RootEffect : AbilityEffect
{
    private void Start()
    {
        visualEffect = PlayerEffect.Root;
    }

    public override void ApplyEffect(Player player, AbilityStats stats, int casterID)
    {
        base.ApplyVisualEffect(player, visualEffect, stats);

        if(player.isNetworkActive)
        {
            Debug.Log("Applying root effect");
            if (stats == null)
                Debug.Log("RootEffect stats are null wtf?");
            if (player.interactionManager == null)
                Debug.Log("RootEffect interactionManager is null wtf?");

            if (player.isNetworkActive)
                player.interactionManager.RootPlayer(stats.duration);

            base.ApplyLocalVisualEffects(player, visualEffect);
        }
    }
}
