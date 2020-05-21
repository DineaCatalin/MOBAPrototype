﻿using UnityEngine;
using System;

// Implements the logic of the collision of the ability projectile
//[RequireComponent(typeof(AbilityData))]
public class AbilityCollider : MonoBehaviour
{
    AbilityData abilityData;
    AbilityEffect effect;

    int casterPlayerID;

    // Will be used to load the AbilityData, has to be the same as the ability Monobehav. that is creating it
    [SerializeField] new string name; 

    // If our ability is static (Roots, Spikes) it should not be destroyed on contact with the player
    // It will also not move so we need a mechanism to destroy it after it's duration is gone
    bool isStatic;

    [HideInInspector] public bool doubleDamageEffect;

    ProjectileVisuals projectileVisuals;

    private void Awake()
    {
        abilityData = AbilityDataCache.GetDataForAbility(name);

        effect = GetComponent<AbilityEffect>();

        if (GetComponent<AbilityDuration>() != null)
        {
            isStatic = true;
        }
        else
        {
            isStatic = false;
        }

        projectileVisuals = GetComponent<ProjectileVisuals>();
    }

    public void SetCasterID(int casterID)
    {
        casterPlayerID = casterID;
    }

    public void ActivateDoubleDamageEffect(bool doubleDamage)
    {
        doubleDamageEffect = doubleDamage;

        abilityData.stats.hpValue *= 2;
        abilityData.stats.dotValue *= 2;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("AbilityCollider OnTriggerEnter2D collision.tag " + collision.tag + " casterTeamName" + abilityData.description.casterTeamName);
        // Check if we've hit the player
        if (collision.tag.Contains("Team"))
        {
            Debug.Log("AbilityCollider OnTriggerEnter2D collided with player");
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
            Deactivate();
        }
        else
        {
            HandleElementalCollisions(collision);
        }
    }

    void HandlePlayerCollision(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();

        effect.ApplyEffect(player, abilityData.stats, casterPlayerID);

        if (!isStatic && abilityData.description.name != "Tornado" && abilityData.description.name != "FireStorm" && abilityData.description.name != "Blast")
        {
            Debug.Log("Destroyng projectile " + this.gameObject.name + " that hit player " + player.GetID());
            Deactivate();
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

        // Deactivate the ability after the collision
        Deactivate();
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
                    Deactivate();
                }
                break;

            case "Tornado":
                if (collision.tag == "Fire Strom" || collision.tag == "Water Rain")
                {
                    collision.gameObject.SetActive(false);
                    Deactivate();
                }
                break;
        }
    }

    void Deactivate()
    {
        projectileVisuals.Deactivate();
        DeactivateDoubleDamage();
    }

    void DeactivateDoubleDamage()
    {
        if (doubleDamageEffect)
        {
            abilityData.stats.hpValue /= 2;
            abilityData.stats.dotValue /= 2;
        }

        doubleDamageEffect = false;
    }
}
