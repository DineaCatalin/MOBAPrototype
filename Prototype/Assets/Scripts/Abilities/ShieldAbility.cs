using UnityEngine;
using System.Collections;

public class ShieldAbility : Ability
{
    // Cache raycast objects, we will use them to see if we are clicking on a player from our team
    Vector2 mousePosition;
    RaycastHit2D hit;

    Player player;

    private void Start()
    {
        Load();

        if(isInstant)
            player = LocalPlayerReferences.player;
    }

    public override bool Cast()
    {
        Debug.Log("Casting Shield ability");

        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if(isInstant)
        {
            player.ActivateShield(abilityData.stats.hpValue);
            return base.Cast();
        }
        else
        {
            hit = Physics2D.Raycast(mousePosition, Vector2.zero, 0);

            if(hit)
            {
                Debug.Log("We clicked on object with tag " + hit.collider.gameObject.tag);

                // We clicked on us or our team mate
                // This means that the object we clicked in in the same layer as our player or team mate
                if (hit.collider.gameObject.layer == gameObject.layer)
                {
                    player = hit.collider.gameObject.GetComponent<Player>();
                    Debug.Log("ShieldAbility activating shield with " + abilityData.stats.hpValue + " through controller for player " + playerID);
                    player.ActivateShield(abilityData.stats.hpValue);

                    return base.Cast();
                }
            }
            
        }

        isCharging = false;
        return false;
    }
}
