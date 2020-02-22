using UnityEngine;
using System.Collections;

public class ProjectileAbility : Ability
{
    // Projectile that will be activated once the ability has been activated
    // By projectile we can also mean static elements like roots or spikes
    // The projectile it self will know if it moves or what it does on collision
    [SerializeField] GameObject projectile;

    [SerializeField] Transform castOrigin;

    public override void Load()
    {
        base.Load();

//        Debug.Log("ProjectileAbility chceking stats name is " + abilityData.description.name);
    }

    private void Start()
    {
        if (castOrigin == null)
        {
            castOrigin = GameObject.Find("CastOrigin" + playerID).transform;
        }
    }

    public override void Cast()
    {
        base.Cast();

        string projectileName = projectile.name.Replace("(Clone)", "");

        AbilitySpawner.Instance.SpawnProjectile(projectileName, castOrigin.position, Quaternion.identity, Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }

}
