using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class GameManager : MonoBehaviour
{
    // TODO: Load this from a config later
    // PLS don't leave it like this
    public static float respawnCooldown = 5f;

    public static GameManager Instance;

    PhotonView photonView;

    Dictionary<int, Player> playerMap;

    // Holds team and score
    Match match;

    private void Awake()
    {
        Instance = this;
        photonView = GetComponent<PhotonView>();
        playerMap = new Dictionary<int, Player>();

        match = new Match();
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

    // Will be called when a player connects to the game
    public void AddPlayer(int playerID)
    {
        Debug.Log("GameManager Adding player " + playerID);
        photonView.RPC("AddPlayerRPC", RpcTarget.All, playerID);
    }

    [PunRPC]
    public void AddPlayerRPC(int playerID)
    {
        Debug.Log("GameManager AddPlayerRPC " + playerID);

        // Find player with playerID. We would have sent the player directly as a parameter to the RPC
        // but it doens't work, so we send the ID and every client has to find the Player by itself
        // We say it's fine because this is triggered only at the beginning of the match when the players are added
        Player player = GameObject.Find("Player" + playerID).GetComponent<Player>();

        playerMap.Add(playerID, player);

        int teamID = GetTeamToAssign();
        match.AssignPlayer(player, teamID);
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

    int GetTeamToAssign()
    {
        int teamID = (int)PhotonNetwork.CurrentRoom.CustomProperties["spawnedPlayerTeamID"];

        if (teamID == 0)
            teamID = 1;
        else
            teamID = (teamID % 2) + 1;

        // Assign the new value to the room properties
        Hashtable roomProperties = new Hashtable();
        roomProperties.Add("spawnedPlayerTeamID", teamID);
        PhotonNetwork.CurrentRoom.SetCustomProperties(roomProperties);

        Debug.Log("GameManager GetTeamToAssign teamID is " + teamID);
        
        return teamID;
    }
}
