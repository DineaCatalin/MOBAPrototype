using UnityEngine;
using System.Collections;

// Implements the logic of the collision of the ability projectile
public class ProjectileCollider : MonoBehaviour
{
    AbilityStats stats;

    // If our ability is a trap (Roots, Spikes) it should not be destroyed on contact with the player
    bool isTrap;

    public string casterTeamName;

    private void Start()
    {
        stats = GetComponent<AbilityStats>();
        
        if(GetComponent<AbilityDuration>() != null)
        {
            isTrap = true;
        }
        else
        {
            isTrap = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if we've hit the player TODO: change later to check if the player is from the other team
        if(collision.tag.Contains("Team") && collision.tag != casterTeamName)
        {
            Player player = collision.GetComponent<Player>();
            player.ApplyAbilityEffect(stats);
            Debug.Log("Projectile has hit " + collision);
            if (!isTrap)
                Destroy(gameObject);
                //gameObject.SetActive(false);

            return;
        }
        // We hit a wall, just deactivate the projectile
        // We also want the traps to not interact with the wall
        else if(collision.tag == "Wall" && !isTrap)
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
        switch(stats.name)
        {
            case "Blast":
                if(collision.tag == "Spikes" || collision.tag == "Roots")
                {
                    collision.gameObject.SetActive(false);
                }
                break;

            case "Tornado":
                if (collision.tag == "Fire Strom" || collision.tag == "Water Rain")
                {
                    collision.gameObject.SetActive(false);
                }
                break;

            case "Fireball":
                if (collision.tag == "Ice Wall")
                {
                    collision.gameObject.SetActive(false);
                }
                break;
        }
    }
}
