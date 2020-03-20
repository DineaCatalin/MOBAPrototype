using UnityEngine;

// This class encapsulates the effect component of an ability
// that will be applied when an ability collides with a player
public abstract class AbilityEffect : MonoBehaviour
{
    public abstract void ApplyEffect(Player player, AbilityStats stats);
}
