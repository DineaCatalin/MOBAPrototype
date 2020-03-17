using UnityEngine;
using System.Collections;

// We will use this to spawn the SpawnedObject at the position of the mouse with the players rotation
public class SpawnAbility : Ability
{
    [SerializeField] GameObject spawnedObject;

    Vector3 spawnPosition;

    [SerializeField] bool mimicPlayerRotation = true;

    Quaternion rotation;

    int projectileLayer;
    [SerializeField] bool isBuffForTeam;

    string projectileName;

    float castRange;

    Transform playerTransform;

    // This will help with the render order of the spawned abilities vs projectiles
    // Projectiles will be rendered above the spawned static abilities
    int zOrder = 2;

    // Use this for initialization
    void Start()
    {
        base.Load();

        // Cache transform to later check for cast range 
        GameObject playerGO = GameObject.Find("Player" + playerID);
        playerTransform = playerGO.transform;

        string layerName = LayerMask.LayerToName(playerGO.layer);
        layerName = layerName.Replace("Player", "Ability");

        projectileName = spawnedObject.name.Replace("(Clone)", "");

        // Load castRange based on projectile name
        string abilityBaseName = projectileName.Replace("Projectile", "");
        Debug.Log("SpawnAbility Start name for cast range config is " + abilityBaseName);
        castRange = AbilityDataCache.GetAbilityCastRange(abilityBaseName);

        // If it's an ice wall or a mana sphere leave the layer to be the default one so that all players can interact with it
        if (name.Contains("IceWall") || name.Contains("Mana"))
            return;

        if (isBuffForTeam)
            layerName = Utils.SwitchPlayerLayerName(layerName);

        projectileLayer = LayerMask.NameToLayer(layerName);
    }

    public override bool Cast()
    {
        base.Cast();

        Debug.Log("SpawnAbility is casting " + abilityData.description.name + " at position " + Utils.GetMousePosition());

        spawnPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        spawnPosition.z = zOrder;

        Debug.Log("Spawnability distance is " + Vector3.Distance(playerTransform.position, spawnPosition) + " cast range is " + castRange);

        // We have exceeded the cast range
        if (Vector2.Distance(playerTransform.position, spawnPosition) > castRange)
        {
            Debug.Log("Spawnability returning");
            return false;
        } 

        Debug.Log("SpawnAbility spellIndicator has ");

        if (mimicPlayerRotation)
            rotation = spellIndicator.transform.rotation;
        else
            rotation = Quaternion.identity;

        AbilitySpawner.Instance.SpawnAbility(projectileName, spawnPosition, rotation, projectileLayer);
        abilityUI.ActivateCooldown();

        return true;
    }
}
