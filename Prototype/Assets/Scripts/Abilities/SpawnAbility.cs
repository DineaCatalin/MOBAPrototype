using UnityEngine;
using System.Collections;

// We will use this to spawn the SpawnedObject at the position of the mouse with the players rotation
public class SpawnAbility : Ability
{
    [SerializeField] GameObject spawnedObject;

    Vector3 spawnPosition;

    //[SerializeField] bool mimicPlayerRotation;

    // Use this for initialization
    void Start()
    {
        base.Load();
    }

    public override void Cast()
    {
        base.Cast();

        Debug.Log("SpawnAbility is casting " + abilityData.description.name + " at position " + Utils.GetMousePosition());

        spawnPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        spawnPosition.z = 0;
            
        Debug.Log("SpawnAbility spellIndicator has ");

        Instantiate(spawnedObject, spawnPosition, spellIndicator.transform.rotation);

        //if(mimicPlayerRotation)
        //    Instantiate(spawnedObject, spawnPosition, spellIndicator.transform.rotation);
        //else
        //    Instantiate(spawnedObject, spawnPosition, Quaternion.identity);
    }
}
