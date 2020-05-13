using UnityEngine;
using System.Collections;

public class BlinkAbility : Ability
{
    //Reference to the player transform so that we can move the player
    Transform playerTransform;
    //Rigidbody2D playerRigidbody;

    // Make sure it's arround 0.05 seconds more then the scale time for the PlayerGraphics
    [SerializeField] float delay = 0.25f; 

    Vector3 blinkPosition;

    float castRange;

    RaycastHit2D hit;

    PlayerTeleportation teleportation;

    private void Start()
    {
        playerTransform = LocalPlayerReferences.playerTransform;
        //playerRigidbody = LocalPlayerReferences.playerRigidbody;
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

        blinkPosition.z = 0;

        Debug.Log("BlinkAbility Deactivating player " + playerID);
        PlayerManager.Instance.DeactivatePlayer(playerID);

        //Blink();
        Invoke("Blink", delay);

        return base.Cast();
    }

    void Blink()
    {
        // Set player position to the mouse position
        teleportation.Teleport(blinkPosition);
        //transform.position = blinkPosition;

        Debug.Log("BlinkAbility Activating player " + playerID);
        PlayerManager.Instance.ActivatePlayerGraphics(playerID);
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
