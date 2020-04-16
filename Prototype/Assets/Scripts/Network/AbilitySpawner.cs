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

    PhotonView photonView;

    public static AbilitySpawner Instance;

    private void Start()
    {
        Instance = this;

        LoadMap();
        photonView = GetComponent<PhotonView>();
    }

    public void SpawnAbility(string name, Vector3 position, Quaternion rotation, int layer, int casterPlayerID)
    {
        photonView.RPC("SpawnStaticAbility", RpcTarget.All, name, position, rotation, layer, casterPlayerID);
    }

    [PunRPC]
    void SpawnStaticAbility(string projectileName, Vector3 position, Quaternion rotation, int layer, int casterPlayerID)
    {
        Debug.Log("AbilitySpawner Spawning " + projectileName);

        GameObject spawned = AbilityProjectilePool.Instance.GetProjectile(projectileName);
        spawned.transform.position = position;
        spawned.transform.rotation = rotation;
        spawned.SetActive(true);

        spawned.layer = layer;

        Player player = GameManager.Instance.GetPlayer(casterPlayerID);
        AbilityCollider abilityCollider = spawned.GetComponent<AbilityCollider>();

        if(abilityCollider)
            abilityCollider.SetCasterID(casterPlayerID);

        if (player.hasDoubleDamage)
        {
            abilityCollider.ActivateDoubleDamageEffect(player.hasDoubleDamage);
        }
    }

    public void SpawnProjectile(string name, Vector3 position, Quaternion rotation, Vector3 direction, int layer, int casterPlayerID)
    {
        photonView.RPC("SpawnProjectileAbility", RpcTarget.All, name, position, rotation, direction, layer, casterPlayerID);
    }

    [PunRPC]
    void SpawnProjectileAbility(string projectileName, Vector3 position, Quaternion rotation, Vector3 direction, int layer, int casterPlayerID)
    {
        GameObject projectile = AbilityProjectilePool.Instance.GetProjectile(projectileName);
        projectile.transform.position = position;
        projectile.transform.rotation = rotation;
        Debug.Log("AbilitySpawner SpawnProjectileAbility position " + position);

        projectile.layer = layer;

        projectile.SetActive(true);

        MoveAbility movement = projectile.GetComponent<MoveAbility>();

        if(movement != null)
            movement.SetDirection(direction);

        Player player = GameManager.Instance.GetPlayer(casterPlayerID);
        AbilityCollider abilityCollider = projectile.GetComponent<AbilityCollider>();

        abilityCollider.SetCasterID(casterPlayerID);

        if (player.hasDoubleDamage)
        {
            abilityCollider.ActivateDoubleDamageEffect(player.hasDoubleDamage);
        }
    }
    
    // We load all the projectiles from the list into the map(Dictionary) and empty the list
    void LoadMap()
    {
        projectileMap = new Dictionary<string, GameObject>();

        AbilityProjectilePool.Instance.InitWithObjectList(projectiles);

        // Clear and remove refference
        projectiles.Clear();
        projectiles = null;
    }
}
