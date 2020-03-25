public class DamageEffect : AbilityEffect
{
    public override void ApplyEffect(Player player, AbilityStats stats)
    {
        if(player.isNetworkActive)
            player.Damage(stats.hpValue);
    }
}
