using UnityEngine;
using System.Collections;

public class HealEffect : AbilityEffect
{
    public override void ApplyEffect(Player player, AbilityStats stats, int casterID)
    {
        base.ApplyLocalVisualEffects(player, visualEffect);
        PlayerManager.Instance.ActivatePlayerUIBuff(visualEffect, stats.duration, player.GetID(), Photon.Pun.RpcTarget.All);

        if (player.isNetworkActive)
            player.HealOverTime(stats.duration, stats.hpValue);
    }
}
