public class ManaBurnEffect : AbilityEffect
{
    public override void ApplyEffect(Player player, AbilityStats stats, int casterID)
    {
        base.ApplyVisualEffect(player, visualEffect, stats);

        if (player.isNetworkActive)
        {
            player.UseMana(stats.dotValue);
            player.Damage(stats.hpValue, casterID);

            base.ApplyLocalVisualEffects(player, visualEffect);
        }
    }
}
