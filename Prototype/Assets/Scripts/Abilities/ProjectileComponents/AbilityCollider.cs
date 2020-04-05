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

    [HideInInspector] public bool doubleDamageEffect;

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

    public void ActivateDoubleDamageEffect(bool doubleDamage)
    {
        doubleDamageEffect = doubleDamage;

        abilityData.stats.hpValue *= 2;
        abilityData.stats.dotValue *= 2;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("collision.tag " + collision.tag + " casterTeamName" + abilityData.description.casterTeamName);
        // Check if we've hit the player
        if (collision.tag.Contains("Team"))
        {
            HandlePlayerCollision(collision);
        }
        // We hit an ice wall so apply some damage
        else if(collision.tag == "Ice Wall")
        {
            Debug.Log("We hit the ice wall");
            HandleIceWallCollision(collision);
        }
        // We hit a wall, just deactivate the projectile
        // We also want the traps to not interact with the wall
        else if(collision.tag == "Wall" && !isStatic)
        {
            Debug.Log("Collided with wall");
            gameObject.SetActive(false);
        }
        else
        {
            HandleElementalCollisions(collision);
        }
    }

    void HandlePlayerCollision(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();

        if (player.isNetworkActive)
        {
            effect.ApplyEffect(player, abilityData.stats);
        }

        if (!isStatic && abilityData.description.name != "Tornado" && abilityData.description.name != "FireStorm")
        {
            Debug.Log("Destroting projectile " + this.gameObject.name + " that hit player " + player.GetID());
            gameObject.SetActive(false);
        }
    }

    void HandleIceWallCollision(Collider2D collision)
    {
        // Ice wall will not interact with the following abilities
        if(abilityData.description.name == "Trace"
           || abilityData.description.name == "Water Rain"
           || abilityData.description.name == "Spikes"
           || abilityData.description.name == "Roots"
           || abilityData.description.name == "Earthquake")
        {
            Debug.Log("AbilityCollider HandleIceWallCollision Ignore " + abilityData.description.name);
            return;
        }

        DestroyAfterCollisions iceWall = collision.GetComponent<DestroyAfterCollisions>();
        iceWall.ApplyDamage();

        // Destroy the ability after the collision
        gameObject.SetActive(false);
    }

    // This will handle the collision between abilities
    // Bad implementation but we are rushing a prototype here...
    void HandleElementalCollisions(Collider2D collision)
    {
        switch (abilityData.description.name)
        {
            case "Blast":
                if (collision.tag == "Spikes" || collision.tag == "Roots")
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
        }
    }

    private void OnDisable()
    {
        if(doubleDamageEffect)
        {
            abilityData.stats.hpValue /= 2;
            abilityData.stats.dotValue /= 2;
        }

        doubleDamageEffect = false;
    }
}
