using UnityEngine;

// This class encapsulates the effect component of an ability
// that will be applied when an ability collides with a player
public abstract class AbilityEffect : MonoBehaviour
{
    public PlayerEffect visualEffect;

    public abstract void ApplyEffect(Player player, AbilityStats stats);

    protected void ApplyVisualEffect(Player player, PlayerEffect visualEffect, AbilityStats stats)
    {
        if (visualEffect == PlayerEffect.None)
            return;

        player.ActivateBuffUI(visualEffect, stats.duration);
    }

    protected void ApplyLocalVisualEffects(Player player, PlayerEffect visualEffect)
    {
        switch(visualEffect)
        {
            case PlayerEffect.Knockout:
                {
                    EventManager.TriggerEvent("KnockOut");
                    break;
                }
            default:
                {
                    return;
                }
        }
            
    }
}

