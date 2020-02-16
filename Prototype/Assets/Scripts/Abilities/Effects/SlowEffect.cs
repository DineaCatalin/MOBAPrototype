using UnityEngine;
using System.Collections;

// Abilities that will slow other players will implement this effect
public class SlowEffect : AbilityEffect
{
    [SerializeField] bool applyDamage;

    public override void ApplyEffect(Player player, AbilityStats stats)
    {
        Debug.Log("SlowEffect");

        player.Slow(stats.duration, stats.dotValue);

        if (applyDamage)
            player.Damage(stats.hpValue);
    }
}
