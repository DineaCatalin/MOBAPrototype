using UnityEngine;
using System.Collections;

public class BlinkAbility : Ability
{
    //Reference to the player transform so that we can move the player
    [SerializeField] Transform playerTransform;

    Vector3 blinkPosition;

    float castRange;

    private void Start()
    {
        if (playerTransform == null)
        {
            // Due to the fact that the player is instantiated over the network the object
            // we are working on now is a clone so it has the string "(Clone)" attached to its name
            playerTransform = GameObject.Find("Player" + playerID).transform;
        }

        // Load cast range from config
        castRange = AbilityDataCache.GetAbilityCastRange("Blink");
    }

    public override bool Cast()
    {
        base.Cast();

        Debug.Log("BlinkAbility Deactivating player " + playerID);

        blinkPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        blinkPosition.z = 0;

        Debug.Log("BlinkAbility distance is " + Vector2.Distance(playerTransform.position, blinkPosition));

        // We have exceeded the cast range
        if (Vector2.Distance(playerTransform.position, blinkPosition) > castRange)
            return false;

        GameManager.Instance.DeactivatePlayer(playerID);

        // Set player position to the mouse position
        playerTransform.position = blinkPosition;

        Debug.Log("BlinkAbility Activating player " + playerID);
        GameManager.Instance.ActivatePlayer(playerID);

        abilityUI.ActivateCooldown();

        return true;
    }

}
