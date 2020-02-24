using UnityEngine;
using System.Collections;

public class KnockoutEffect : AbilityEffect
{
    public override void ApplyEffect(Player player, AbilityStats stats)
    {
		//player.Knockout(stats.dotValue, stats.hpValue);
		GameManager.Instance.KnockOutPlayer(stats.dotValue, stats.hpValue, player.GetID());
    }
}
