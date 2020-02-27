using UnityEngine;
using System.Collections;
using Photon.Pun;
using System.Collections.Generic;

// We will use this to spawn all the projectiles
public class AbilitySpawner : MonoBehaviour
{
    // Load from the inspector
    public List<GameObject> projectiles;

    Dictionary<string, GameObject> projectileMap;

    //Dictionary<int, GameObject> activeProjectiles;

    PhotonView photonView;

    public static AbilitySpawner Instance;

    private void Start()
    {
        Instance = this;

        LoadMap();
        photonView = GetComponent<PhotonView>();
    }

    public void SpawnAbility(string name, Vector3 position, Quaternion rotation, int layer)
    {
        photonView.RPC("SpawnStaticAbility", RpcTarget.All, name, position, rotation, layer);
    }

    [PunRPC]
    void SpawnStaticAbility(string projectileName, Vector3 position, Quaternion rotation, int layer)
    {
        Debug.Log("AbilitySpawner Spawn rotation is " + rotation);
        GameObject spawned = Instantiate(projectileMap[projectileName], position, rotation);
        spawned.layer = layer;
    }

    public void SpawnProjectile(string name, Vector3 position, Quaternion rotation, Vector3 direction, int layer)
    {
        photonView.RPC("SpawnProjectileAbility", RpcTarget.All, name, position, rotation, direction, layer);
    }

    [PunRPC]
    void SpawnProjectileAbility(string projectileName, Vector3 position, Quaternion rotation, Vector3 direction, int layer)
    {
        Debug.Log("AbilitySpawner Spawn rotation is " + rotation);

        GameObject projectile = Instantiate(projectileMap[projectileName], position, rotation);
        projectile.layer = layer;

        MoveAbility movement = projectile.GetComponent<MoveAbility>();

        if(movement != null)
            movement.SetDirection(direction);

        //activeProjectiles.Add(projectile.GetInstanceID(), projectile);
    }

    // We load all the projectiles from the list into the map(Dictionary) and empty the list
    void LoadMap()
    {
        projectileMap = new Dictionary<string, GameObject>();

        // Load the Dictionary
        foreach (var projectile in projectiles)
        {
            Debug.Log("AbilitySpawner adding projectile with name " + projectile.name);
            projectileMap.Add(projectile.name, projectile);
        }

        // Clear and remove refference
        projectiles.Clear();
        projectiles = null;
    }
}
