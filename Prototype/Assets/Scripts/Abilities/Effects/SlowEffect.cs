using UnityEngine;
using System.Collections;

// Abilities that will slow other players will implement this effect
public class SlowEffect : AbilityEffect
{
    [SerializeField] bool applyDamage;

    private void Start()
    {
        visualEffect = PlayerEffect.Slow;
    }

    public override void ApplyEffect(Player player, AbilityStats stats, int casterID)
    {
        Debug.Log("SlowEffect");
        base.ApplyVisualEffect(player, visualEffect, stats);

        if (player.isNetworkActive)
        {
            if (applyDamage)
            {
                player.SlowForDuration(stats.dotValue, stats.duration);
                player.Damage(stats.hpValue, casterID);
            }
            else
            {
                player.SlowForDuration(stats.dotValue, stats.duration);
            }

            base.ApplyLocalVisualEffects(player, visualEffect);
        }
    }
}
