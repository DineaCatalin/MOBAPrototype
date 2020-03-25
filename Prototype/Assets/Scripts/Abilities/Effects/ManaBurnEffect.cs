public class ManaBurnEffect : AbilityEffect
{
    public override void ApplyEffect(Player player, AbilityStats stats)
    {
        if(player.isNetworkActive)
        {
            player.UseMana(stats.dotValue);
            player.Damage(stats.hpValue);
        }
    }
}
