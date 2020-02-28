using UnityEngine;
using System.Collections;

// Implements the logic of the collision of the ability projectile
//[RequireComponent(typeof(AbilityData))]
public class AbilityCollider : MonoBehaviour
{
    AbilityData abilityData;
    AbilityEffect effect;

    // Will be used to load the AbilityData, has to be the same as the ability Monobehav. that is creating it
    [SerializeField] new string name; 

    // If our ability is static (Roots, Spikes) it should not be destroyed on contact with the player
    // It will also not move so we need a mechanism to destroy it after it's duration is gone
    bool isStatic;

    private void Awake()
    {
        abilityData = AbilityDataCache.GetDataForAbility(name);

        effect = GetComponent<AbilityEffect>();

        if (GetComponent<AbilityDuration>() != null || GetComponent<Flicker>() != null)
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
        if (collision.tag.Contains("Team"))
        {
            Player player = collision.GetComponent<Player>();
            effect.ApplyEffect(player, abilityData.stats);
            Debug.Log("Projectile has hit " + collision);

            if (!isStatic && abilityData.description.name != "Tornado")
            {
                Debug.Log("Destroting projectile " + this.gameObject.name + " that hit player " + player.GetID());
                Destroy(this.gameObject);
            }

            return;
        }
        // We hit an ice wall so apply some damage
        else if(collision.tag == "Ice Wall")
        {
            Debug.Log("We hit the ice wall");
            DestroyAfterCollisions iceWall = collision.GetComponent<DestroyAfterCollisions>();

            if (abilityData.description.name == "Fireball")
                iceWall.Destroy();
            else
                iceWall.ApplyDamage();

            // Destroy the ability after the collision
            gameObject.SetActive(false);
            Destroy(gameObject);

            return;
        }
        // We hit a wall, just deactivate the projectile
        // We also want the traps to not interact with the wall
        else if(collision.tag == "Wall" && !isStatic)
        {
            Debug.Log("Collided with wall");
            Destroy(this.gameObject);
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
                    Destroy(this.gameObject);
                }
                break;

            case "Tornado":
                if (collision.tag == "Fire Strom" || collision.tag == "Water Rain")
                {
                    collision.gameObject.SetActive(false);
                    Destroy(this.gameObject);
                }
                break;
        }
    }
}
