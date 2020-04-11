public class ManaBurnEffect : AbilityEffect
{
    public override void ApplyEffect(Player player, AbilityStats stats)
    {
        base.ApplyVisualEffect(player, visualEffect, stats);

        if (player.isNetworkActive)
        {
            player.UseMana(stats.dotValue);
            player.Damage(stats.hpValue);

            base.ApplyLocalVisualEffects(player, visualEffect);
        }
    }
}
