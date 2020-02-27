using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class GameManager : MonoBehaviour
{
    // TODO: Load this from a config later
    // PLS don't leave it like this
    public static float RESPAWN_COOLDOWN = 5f;
    public static float MAX_PLAYERS = 4;

    const float ADD_PLAYER_DELAY = 1f;

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

    public void AddPlayer(int playerID)
    {
        photonView.RPC("AddPlayerRPC", RpcTarget.All, playerID);
    }

    [PunRPC]
    public void AddPlayerRPC(int playerID)
    {
        Debug.Log("GameManager AddPlayerRPC " + playerID);

        // Find player with playerID. We would have sent the player directly as a parameter to the RPC
        // but it doens't work, so we send the ID and every client has to find the Player by itself
        // We say it's fine because this is triggered only at the beginning of the match when the players are added
        GameObject player = GameObject.Find("Player" + playerID);

        if(player == null)
        {
            StartCoroutine(AddPlayerDelayed(playerID, ADD_PLAYER_DELAY));
        }
        else
        {
            AddPlayerNoDelay(playerID);
        }
    }

    // We will call this coroutine so that the game has time to instantiate the player
    // so that we can find it 
    IEnumerator AddPlayerDelayed(int playerID, float delay)
    {
        yield return new WaitForSeconds(delay);

        Debug.Log("GameManager AddPlayerDelayed " + playerID);

        AddPlayerNoDelay(playerID);
    }

    void AddPlayerNoDelay(int playerID)
    {
        Player player = GameObject.Find("Player" + playerID).GetComponent<Player>();

        int teamID = GetTeamToAssign();
        match.AssignPlayer(player, teamID);

        if (!playerMap.ContainsKey(playerID))
        {
            playerMap.Add(playerID, player);

            // Make sure the other clients have all the players added to their local map
            SyncPlayerMap();
        }
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

    // The controller will take care of passing the message to the player to activate it's shield
    // from the ShieldAbility. It will do this because the photonView is attached to the GO the controller is attached,
    // so we can't call RPC's from the components on the child object aka the Player component
    public void ActivatePlayerShield(int armor, int playerID)
    {
        Debug.Log("GameManager ActivatePlayerShield with " + armor + " armor for player " + playerID);
        photonView.RPC("ActivatePlayerShieldRPC", RpcTarget.All, armor, playerID);
    }

    [PunRPC]
    void ActivatePlayerShieldRPC(int armor, int playerID)
    {
        Debug.Log("GameManager ActivatePlayerShieldRPC with " + armor + " armor for player " + playerID);
        if (playerMap[playerID] == null)
            return;

        playerMap[playerID].ActivateShield(armor);
    }

    public void KnockOutPlayer(int force, int damage, int playerID)
    {
        Debug.Log("GameManager KnockOutPlayer");
        playerMap[playerID].Knockout(force, damage);
    }

    public void RemovePlayer(int playerID)
    {
        photonView.RPC("RemovePlayerRPC", RpcTarget.All, playerID);
    }

    [PunRPC]
    void RemovePlayerRPC(int playerID)
    {
        if(playerMap.ContainsKey(playerID))
        {
            Debug.Log("Removing from playerMap player : " + playerID);
            playerMap.Remove(playerID);
        }
    }

    void SyncPlayerMap()
    {
        int[] playerIDs = new int[playerMap.Count];
        int index = 0;

        // Get all the players we have in our map locally
        foreach (KeyValuePair<int, Player> mapValue in playerMap)
        {
            playerIDs[index] = mapValue.Key;
            index++;
        }

        photonView.RPC("SyncPlayerMapRPC", RpcTarget.Others, playerIDs);
    }

    [PunRPC]
    void SyncPlayerMapRPC(int[] playerIDs)
    {
        int playerID;

        // Loop all the ids in the map to see if our local map has all of them
        for (int i = 0; i < playerIDs.Length; i++)
        {
            playerID = playerIDs[i];

            // We found an ID that is not in our local map
            if (!playerMap.ContainsKey(playerID))
            {
                Debug.Log("GameManager SyncPlayerMapRPC Finding player with ID " + playerID);
                GameObject playerGO = GameObject.Find("Player" + playerID);

                if(playerGO != null)
                {
                    Player player = playerGO.GetComponent<Player>();
                    playerMap.Add(playerID, player);
                }
            }
        }
    }

    public void ActivateRushArea(float duration, int playerID)
    {
        photonView.RPC("ActivateRushAreaRPC", RpcTarget.All, duration, playerID);
    }

    [PunRPC]
    void ActivateRushAreaRPC(float duration, int playerID)
    {
        if(playerMap.ContainsKey(playerID))
        {
            playerMap[playerID].ActivateRushArea(duration);
        }
    }
}
