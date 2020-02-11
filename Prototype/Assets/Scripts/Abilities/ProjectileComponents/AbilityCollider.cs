using UnityEngine;
using System.Collections;

// Implements the logic of the collision of the ability projectile
[RequireComponent(typeof(AbilityData))]
public class AbilityCollider : MonoBehaviour
{
    AbilityData abilityData;
    AbilityEffect effect;

    // If our ability is static (Roots, Spikes) it should not be destroyed on contact with the player
    // It will also not move so we need a mechanism to destroy it after it's duration is gone
    bool isStatic;

    private void Start()
    {
        abilityData = GetComponent<AbilityData>();
        effect = GetComponent<AbilityEffect>();

        if (GetComponent<AbilityDuration>() != null)
        {
            isStatic = true;
        }
        else
        {
            isStatic = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("collision.tag " + collision.tag + " casterTeamName" + abilityData.description.casterTeamName);
        // Check if we've hit the player
        if (collision.tag.Contains("Team") && collision.tag != abilityData.description.casterTeamName)
        {
            Player player = collision.GetComponent<Player>();
            //player.ApplyAbilityEffect(abilityData.stats);
            effect.ApplyEffect(player, abilityData.stats);
            Debug.Log("Projectile has hit " + collision);
            if (!isStatic)
                gameObject.SetActive(false);

            return;
        }
        // We hit a wall, just deactivate the projectile
        // We also want the traps to not interact with the wall
        else if(collision.tag == "Wall" && !isStatic)
        {
            Debug.Log("Collided with wall");
            gameObject.SetActive(false);
            return;
        }

        HandleElementalCollisions(collision);
    }

    // This will handle the collision between abilities
    // Bad implementation but we are rushing a prototype here...
    void HandleElementalCollisions(Collider2D collision)
    {
        switch(abilityData.description.name)
        {
            case "Blast":
                if(collision.tag == "Spikes" || collision.tag == "Roots")
                {
                    collision.gameObject.SetActive(false);
                    gameObject.SetActive(false);
                }
                break;

            case "Tornado":
                if (collision.tag == "Fire Strom" || collision.tag == "Water Rain")
                {
                    collision.gameObject.SetActive(false);
                    gameObject.SetActive(false);
                }
                break;

            case "Fireball":
                if (collision.tag == "Ice Wall")
                {
                    collision.gameObject.SetActive(false);
                    gameObject.SetActive(false);
                }
                break;
        }
    }
}
