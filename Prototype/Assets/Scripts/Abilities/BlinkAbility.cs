using UnityEngine;
using System.Collections;

public class BlinkAbility : Ability
{
    //Reference to the player transform so that we can move the player
    [SerializeField] Transform playerTransform;

    Vector3 blinkPosition;

    public override void Cast()
    {
        base.Cast();

        blinkPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        blinkPosition.z = 0;

        // Set player position to the mouse position
        playerTransform.position = blinkPosition;
    }

}
