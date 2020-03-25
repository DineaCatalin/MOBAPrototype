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
            //GameManager.Instance.SlowAndDamagePlayer(stats.duration, stats.dotValue, stats.hpValue, player.GetID());
            player.Slow(stats.dotValue, stats.duration, true);
            player.Damage(stats.hpValue);
        }
        else
        {
            //GameManager.Instance.SlowPlayer(stats.duration, stats.dotValue, player.GetID());
            player.Slow(stats.dotValue, stats.duration);
        }
    }
}
