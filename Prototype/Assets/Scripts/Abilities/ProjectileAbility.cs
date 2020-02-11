using UnityEngine;
using System.Collections;

public class ProjectileAbility : Ability
{
    // Projectile that will be activated once the ability has been activated
    // By projectile we can also mean static elements like roots or spikes
    // The projectile it self will know if it moves or what it does on collision
    [SerializeField] GameObject projectile;

    public override void Cast()
    {
        base.Cast();

        projectile.transform.position = transform.position;
        projectile.SetActive(true);
    }
}
