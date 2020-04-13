using Photon.Pun;

public class DamageEffect : AbilityEffect
{
    public override void ApplyEffect(Player player, AbilityStats stats, int casterID)
    {
        base.ApplyVisualEffect(player, visualEffect, stats);

        if (player.isNetworkActive)
        {
            player.Damage(stats.hpValue, casterID);
            base.ApplyVisualEffect(player, visualEffect, stats);
        }
            
    }
}
