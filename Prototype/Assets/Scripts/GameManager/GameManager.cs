using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;

public class GameManager : MonoBehaviour
{
    // TODO: Load this from a config later
    // PLS don't leave it like this
    public static float respawnCooldown = 5f;

    public static GameManager Instance;

    PhotonView photonView;

    Dictionary<int, Player> playerMap;

    private void Awake()
    {
        Instance = this;
        photonView = GetComponent<PhotonView>();
        playerMap = new Dictionary<int, Player>();
    }

    public void KillAndRespawnPlayer(float respawnTimer, Player player)
    {
        StartCoroutine(SpawnPlayerWithDelay(respawnTimer, player));
    }

    IEnumerator SpawnPlayerWithDelay(float respawnTimer, Player player)
    {
        // Wait respawntimer out
        yield return new WaitForSeconds(respawnTimer);

        // "Respawn" player by activating the player's GO and reset health and mana
        // The player will take care of doing this
        player.Reset();
    }

    public void AddPlayer(Player player)
    {
        playerMap.Add(player.GetID(), player);
    }

    public Player GetPlayer(int playerID)
    {
        return playerMap[playerID];
    }

    public void ActivatePlayer(int playerID)
    {
        photonView.RPC("ActivatePlayerRPC", RpcTarget.All, playerID);
    }

    [PunRPC]
    public void ActivatePlayerRPC(int playerID)
    {
        Debug.Log("ActivatePlayerRPC getting player " + playerID + " from map");
        playerMap[playerID].Activate();
    }

    public void DeactivatePlayer(int playerID)
    {
        photonView.RPC("DeactivatePlayerRPC", RpcTarget.All, playerID);
    }

    [PunRPC]
    public void DeactivatePlayerRPC(int playerID)
    {
        playerMap[playerID].Deactivate();
    }
}
