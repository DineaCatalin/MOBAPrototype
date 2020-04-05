using UnityEngine;
using System.Collections;

// Abilities that will slow other players will implement this effect
public class SlowEffect : AbilityEffect
{
    [SerializeField] bool applyDamage;

    public override void ApplyEffect(Player player, AbilityStats stats)
    {
        Debug.Log("SlowEffect");

        if (applyDamage)
        {
            player.SlowForDuration(stats.dotValue, stats.duration);
            player.Damage(stats.hpValue);
        }
        else
        {
            player.SlowForDuration(stats.dotValue, stats.duration);
        }
    }
}
