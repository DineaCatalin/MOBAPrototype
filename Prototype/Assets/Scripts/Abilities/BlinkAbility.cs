using UnityEngine;
using System.Collections;

public class BlinkAbility : Ability
{
    //Reference to the player transform so that we can move the player
    Transform playerTransform;

    Vector2 blinkPosition;

    float castRange;

    RaycastHit2D hit;

    PlayerTeleportation teleportation;

    private void Start()
    {
        playerTransform = LocalPlayerReferences.playerTransform;
        teleportation = LocalPlayerReferences.player.teleportation;

        // Load cast range from config
        castRange = AbilityDataCache.GetAbilityCastRange("Blink");
    }

    public override bool Cast()
    {
        if (PlayerController.isRooted)
        {
            // update some helper text here

            return false;
        }

        blinkPosition = Utils.Instance.GetMousePosition();

        Debug.Log("BlinkAbility distance is " + Vector2.Distance(playerTransform.position, blinkPosition));

        if (OutOfRange() || HitWall())
            return false;

        Debug.Log("BlinkAbility Calling BlinkNetworkedPlayer player " + playerID);

        PlayerManager.Instance.BlinkNetworkedPlayer(playerID, blinkPosition);

        return base.Cast();
    }

    bool OutOfRange()
    {
        if (Vector2.Distance(playerTransform.position, blinkPosition) > castRange)
            return true;

        return false;            
    }

    bool HitWall()
    {
        // Check if we pressed on a wall so that we don't blink into it
        hit = Physics2D.Raycast(blinkPosition, Vector2.zero);

        if (hit)
        {
            if (hit.collider.gameObject.tag == "Wall")
            {
                return true;
            }
        }

        return false;
    }
}
