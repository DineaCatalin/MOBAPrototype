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

    //Vector3 direction;
    //float angle;
    //int angleCorrection = 90;

    // Use this for initialization
    void Start()
    {
        base.Load();

        // Cache transform to later check for cast range 
        playerTransform = LocalPlayerReferences.playerTransform;

        string layerName = LayerMask.LayerToName(LocalPlayerReferences.playerGameObject.layer);
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
            layerName = Utils.Instance.SwitchPlayerLayerName(layerName);

        projectileLayer = LayerMask.NameToLayer(layerName);
    }

    public override bool Cast()
    {
        Debug.Log("SpawnAbility is casting " + abilityData.description.name + " at position " + Utils.Instance.GetMousePosition());

        spawnPosition = Utils.Instance.GetMousePosition();
        spawnPosition.z = zOrder;

        Debug.Log("Spawnability distance is " + Vector3.Distance(playerTransform.position, spawnPosition) + " cast range is " + castRange);

        // We have exceeded the cast range
        if (Vector2.Distance(playerTransform.position, spawnPosition) > castRange)
        {
            Debug.Log("Spawnability Cast out of range");
            MouseHandler.Instance.ShowOutOfRangeIndicator();
            return false;
        } 

        Debug.Log("SpawnAbility spellIndicator has ");

        if (mimicPlayerRotation)
        {
            //direction = Input.mousePosition - Utils.Instance.CameraScreenToWorldPoint(playerTransform.position);
            //angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - angleCorrection;  // Angle correction as the sprites rotates 90 degrees extra
            //rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            rotation = playerTransform.rotation;
            Debug.Log("SpawnAbility Cast rotation " + rotation);
        }
        else
            rotation = Quaternion.identity;

        AbilitySpawner.Instance.SpawnAbility(projectileName, spawnPosition, rotation, projectileLayer, playerID);
        
        return base.Cast();
    }
}
