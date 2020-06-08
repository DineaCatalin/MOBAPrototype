using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLogic : MonoBehaviour
{
    MoveAbility move;
    AbilityCollider abilityCollider;

    Player player;

    // Start is called before the first frame update
    void Awake()
    {
        move = GetComponent<MoveAbility>();
        abilityCollider = GetComponent<AbilityCollider>();
    }

    public void SetDynamicProjectileSettings(int casterID, int layer, Vector3 direction)
    {
        SetLayer(layer);
        PrepareAbilityCollider(casterID);
        SetMovementDirection(direction);
    }

    public void SetStaticProjectileSettings(int casterID, int layer)
    {
        SetLayer(layer);
        PrepareAbilityCollider(casterID);
    }

    void SetLayer(int layer)
    {
        gameObject.layer = layer;
    }

    void PrepareAbilityCollider(int casterID)
    {
        player = PlayerManager.Instance.GetPlayer(casterID);

        if (!abilityCollider)
            return;

        abilityCollider.SetCasterID(casterID);

        if (player.hasDoubleDamage)
        {
            abilityCollider.ActivateDoubleDamageEffect(player.hasDoubleDamage);
        }
    }

    void SetMovementDirection(Vector3 direction)
    {
        if(move)
            move.SetDirection(direction);
    }
}
