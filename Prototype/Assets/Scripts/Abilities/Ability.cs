using UnityEngine;
using System.Collections;

public class Ability : MonoBehaviour
{
    // This will tell us if the ability will be triggered directly when the user presses the specific hot-key
    // An example like this would be the 
    [SerializeField] bool instaCast;

    public string casterTeamName;

    // Stats of our ability, will be loaded from a config file
    //[SerializeField] AbilityStats stats;

    [SerializeField] GameObject projectileTemplate;
    Transform projectileTransform;

    private void Start()
    {
        projectileTemplate.GetComponent<ProjectileCollider>().casterTeamName = casterTeamName;
        projectileTransform = projectileTemplate.transform;
    }

    // Spawns the projectile with a given rotation
    public void Cast(Vector3 position, Quaternion rotation)
    {
        //projectileTransform.position = position;
        //projectileTransform.rotation = rotation;
        //projectile.SetActive(true);

        var proj = Instantiate(projectileTemplate, position, rotation);
        proj.SetActive(true);
    }

    // Handles what our ability is actually doing
    // This is the version where our ability doesn't spawn a projectile
    public void Cast()
    {

    }

}
