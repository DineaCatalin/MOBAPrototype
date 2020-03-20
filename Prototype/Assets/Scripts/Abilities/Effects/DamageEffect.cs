public class DamageEffect : AbilityEffect
{
    public override void ApplyEffect(Player player, AbilityStats stats)
    {
        player.Damage(stats.hpValue);
    }
}
