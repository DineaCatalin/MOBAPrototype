public class DamageEffect : AbilityEffect
{
    public override void ApplyEffect(Player player, AbilityStats stats)
    {
        base.ApplyVisualEffect(player, visualEffect, stats);

        if (player.isNetworkActive)
        {
            player.Damage(stats.hpValue);
            base.ApplyVisualEffect(player, visualEffect, stats);
        }
            
    }
}
