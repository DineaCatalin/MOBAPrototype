using UnityEngine;
using System.Collections;

public class BlinkAbility : Ability
{
    //Reference to the player transform so that we can move the player
    Transform playerTransform;

    public override void Cast()
    {
        base.Cast();

        // Set player position to the mouse position
        playerTransform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}
