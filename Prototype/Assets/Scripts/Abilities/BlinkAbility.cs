using UnityEngine;
using System.Collections;

public class BlinkAbility : Ability
{
    //Reference to the player transform so that we can move the player
    [SerializeField] Transform playerTransform;

    // Make sure it's arround 0.05 seconds more then the scale time for the PlayerGraphics
    [SerializeField] float delay = 0.30f; 

    Vector3 blinkPosition;

    float castRange;

    RaycastHit2D hit;

    private void Start()
    {
        if (playerTransform == null)
        {
            // Due to the fact that the player is instantiated over the network the object
            // we are working on now is a clone so it has the string "(Clone)" attached to its name
            playerTransform = LocalPlayerReferences.playerTransform;
        }

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

        Invoke("Blink", delay);

        return base.Cast();
    }

    void Blink()
    {
        // Set player position to the mouse position
        playerTransform.position = blinkPosition;

        Debug.Log("BlinkAbility Activating player " + playerID);
        PlayerManager.Instance.ActivatePlayer(playerID);
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
