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

    // Use this for initialization
    void Start()
    {
        base.Load();

        string layerName = LayerMask.LayerToName(GameObject.Find("Player" + playerID).layer);
        layerName = layerName.Replace("Player", "Ability");

        projectileName = spawnedObject.name.Replace("(Clone)", "");

        // If it's an ice wall leave the layer to be the default one so that all players can interact with it
        if (name.Contains("IceWall"))
            return;

        if (isBuffForTeam)
            layerName = Utils.SwitchPlayerLayerName(layerName);

        projectileLayer = LayerMask.NameToLayer(layerName);
    }

    public override void Cast()
    {
        base.Cast();

        Debug.Log("SpawnAbility is casting " + abilityData.description.name + " at position " + Utils.GetMousePosition());

        spawnPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        spawnPosition.z = 1;
            
        Debug.Log("SpawnAbility spellIndicator has ");

        if (mimicPlayerRotation)
            rotation = spellIndicator.transform.rotation;
        else
            rotation = Quaternion.identity;

        AbilitySpawner.Instance.SpawnAbility(projectileName, spawnPosition, rotation, projectileLayer);
    }

    
}
