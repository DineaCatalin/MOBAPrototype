using UnityEngine;
using System.Collections;

public class ProjectileAbility : Ability
{
    // Projectile that will be activated once the ability has been activated
    // By projectile we can also mean static elements like roots or spikes
    // The projectile it self will know if it moves or what it does on collision
    [SerializeField] GameObject projectile;

    // Transform of the location where we will spawn
    Transform castOrigin;

    // Position where we will spawn, we use this varibale because we want to change the z value
    // without affecting the players z value
    Vector3 castPosition;

    [SerializeField] bool mimicPlayerRotation = false;
    [SerializeField] bool invertRotation = false;

    [Tooltip("If true will use the cast origin that is a child of the player, otherwise it will use the players position")]
    [SerializeField] bool useCastOrigin = true;

    int projectileLayer;
    string projectileName;

    // Used when we want to spawn a projectile facing the mouse direction
    //Vector3 spawnDirection;
    //float spawnAngle;
    Quaternion spawnRotation;

    // This will help with the render order of the spawned abilities vs projectiles
    // Projectiles will be rendered above the spawned static abilities
    int zOrder = 1;

    public override void Load()
    {
        base.Load();
    }

    private void Start()
    {
        if (useCastOrigin)
        {
            castOrigin = GameObject.Find("CastOrigin" + playerID).transform;
        }
        else
        {
            castOrigin = GameObject.Find("Player" + playerID).transform;
        }

        // Get layer name for the projectile
        // If its the player is in team1 the layer name will be Team1Ability if player is in team2 the layer name will be Team2Ability
        string layerName = LayerMask.LayerToName(GameObject.Find("Player" + playerID).layer);
        layerName = layerName.Replace("Player", "Ability");

        projectileLayer = LayerMask.NameToLayer(layerName);
        projectileName = projectile.name.Replace("(Clone)", "");
    }

    public override bool Cast()
    {
        Debug.Log("ProjectileAbility Cast casting " + projectileName);

        if (mimicPlayerRotation)
            spawnRotation = spellIndicator.transform.rotation;
        else
            spawnRotation = Quaternion.identity;

        if (invertRotation)
            spawnRotation *= Quaternion.Euler(0, 0, 180f);

        castPosition = new Vector3(castOrigin.position.x, castOrigin.position.y, zOrder);

        AbilitySpawner.Instance.SpawnProjectile(projectileName, castPosition, spawnRotation, Camera.main.ScreenToWorldPoint(Input.mousePosition), projectileLayer);

        return base.Cast();
    }

}
