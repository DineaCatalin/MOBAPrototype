public class ManaBurnEffect : AbilityEffect
{
    public override void ApplyEffect(Player player, AbilityStats stats)
    {
        player.UseMana(stats.dotValue);
        player.Damage(stats.hpValue);
    }
}
