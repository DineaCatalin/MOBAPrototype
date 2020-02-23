using UnityEngine;
using System.Collections;

public class BlinkAbility : Ability
{
    //Reference to the player transform so that we can move the player
    [SerializeField] Transform playerTransform;

    Vector3 blinkPosition;

    private void Start()
    {
        if (playerTransform == null)
        {
            // Due to the fact that the player is instantiated over the network the object
            // we are working on now is a clone so it has the string "(Clone)" attached to its name
            playerTransform = GameObject.Find("Player" + playerID).transform;
        }
    }

    public override void Cast()
    {
        base.Cast();

        Debug.Log("BlinkAbility Deactivating player " + playerID);
        GameManager.Instance.DeactivatePlayer(playerID);

        blinkPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        blinkPosition.z = 0;

        // Set player position to the mouse position
        playerTransform.position = blinkPosition;

        Debug.Log("BlinkAbility Activating player " + playerID);
        GameManager.Instance.ActivatePlayer(playerID);
    }

}
