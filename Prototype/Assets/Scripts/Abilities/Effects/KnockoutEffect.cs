using UnityEngine;
using System.Collections;

public class KnockoutEffect : AbilityEffect
{
    public override void ApplyEffect(Player player, AbilityStats stats)
    {
		//GameManager.Instance.KnockOutPlayer(stats.dotValue, stats.hpValue, player.GetID());
        if(player.isNetworkActive)
            player.Knockout(stats.dotValue, stats.hpValue);
    }
}
