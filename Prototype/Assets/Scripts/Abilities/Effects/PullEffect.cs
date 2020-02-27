using UnityEngine;
using System.Collections;

public class PullEffect : AbilityEffect
{
    public override void ApplyEffect(Player player, AbilityStats stats)
    {
        //player.PullToLocation(transform.position, stats.dotValue, stats.hpValue);
        GameManager.Instance.PullPlayer(transform.position, stats.dotValue, stats.hpValue, player.GetID());
    }
}
