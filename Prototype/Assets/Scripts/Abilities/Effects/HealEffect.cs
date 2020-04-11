using UnityEngine;
using System.Collections;

public class HealEffect : AbilityEffect
{
    public override void ApplyEffect(Player player, AbilityStats stats)
    {
        base.ApplyLocalVisualEffects(player, visualEffect);
        player.HealOverTime(stats.duration, stats.hpValue);
        GameManager.Instance.ActivatePlayerUIBuff(visualEffect, stats.duration, player.GetID(), Photon.Pun.RpcTarget.All);
    }
}
