using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class GameManager : MonoBehaviour
{
    // TODO: Load this from a config later
    // PLS don't leave it like this
    public static float RESPAWN_COOLDOWN = 3f;
    public static float MAX_PLAYERS = 4f; 

    public static float TIME_BETWEEN_ROUNDS = 3f;

    const float ADD_PLAYER_DELAY = 1f;

    public static GameManager Instance;

    PhotonView photonView;

    Dictionary<int, Player> playerMap;

    // Holds team and score
    Match match;

    // Total players in this game
    int totalPlayers;
    int playersReady;

    private void Awake()
    {
        Instance = this;
        photonView = GetComponent<PhotonView>();
        playerMap = new Dictionary<int, Player>();

        playersReady = 0;

        EventManager.StartListening("RoundEnd", new System.Action(OnRoundEnd));
        EventManager.StartListening("StartRedraft", new System.Action(OnStartRedraft)); 
        EventManager.StartListening("EndRedraft", new System.Action(OnRedraftEnd));

        match = new Match();
        Hashtable roomProperties = new Hashtable();

        if (PhotonNetwork.IsMasterClient)
        {
            // Assign the new value to the room properties
            roomProperties.Add("Team1Rounds", 0);
            roomProperties.Add("Team2Rounds", 0);
            PhotonNetwork.CurrentRoom.SetCustomProperties(roomProperties);
        }
    }

    // TODO: Rework this logic later
    bool matchStarted = false;
    private void Update()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            if (Input.GetKeyDown(KeyCode.Space) && !matchStarted)
            {
                StartMatch();

                photonView.RPC("StartMatchRPC", RpcTarget.Others);
                matchStarted = true;

                // Close the room so that other players can't connect
                PhotonNetwork.CurrentRoom.IsOpen = false;
            }
        }
    }

    public void ActivateNonLocalPlayer(int playerID)
    {
        photonView.RPC("ActivateNonLocalPlayerRPC", RpcTarget.Others, playerID);
    }

    [PunRPC]
    void ActivateNonLocalPlayerRPC(int playerID)
    {
        // TODO: recheck
        StartCoroutine(ActivateNonLocalPlayerCoroutine(playerID, 0.8f));
    }

    IEnumerator ActivateNonLocalPlayerCoroutine(int playerID, float delay)
    {
        yield return new WaitForSeconds(delay);

        Player player = playerMap[playerID];

        if (player != null && !player.isNetworkActive)
        {
            player.Activate();
        }
    }

    void OnStartRedraft()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("StartRedraftRPC", RpcTarget.Others);
        }

        match.SyncScore();
    }

    [PunRPC]
    void StartRedraftRPC()
    {
        EventManager.TriggerEvent("StartRedraft");
    }

    void OnRedraftEnd()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            CheckRedraftFinished();
        }
        else
        {
            photonView.RPC("CheckRedraftFinishedRPC", RpcTarget.MasterClient);
        }
    }

    [PunRPC]
    void CheckRedraftFinishedRPC()
    {
        CheckRedraftFinished();
    }

    void CheckRedraftFinished()
    {
        playersReady++;

        if(playersReady >= totalPlayers)
        {
            OnRoundEnd();
        }
    }

    void OnRoundEnd()
    {
        Debug.Log("GameManager OnRoundEnd");
        EndRound();
        photonView.RPC("RoundEndRPC", RpcTarget.Others);
    }

    [PunRPC]
    public void RoundEndRPC()
    {
        EndRound();
    }

    void EndRound()
    {
        // Sync match scores
        match.SyncScore();

        StartCoroutine("StartRoundWithDelay");
    }

    IEnumerator StartRoundWithDelay()
    {
        yield return new WaitForSeconds(TIME_BETWEEN_ROUNDS);

        Debug.Log("GameManager StartRoundWithDelay Starting round");
        EventManager.TriggerEvent("StartRound");
    }

    //// Handle match end logic
    public void EndMatch(int winnerTeamID)
    {

    }

    [PunRPC]
    public void StartMatchRPC()
    {
        StartMatch();
    }

    void StartMatch()
    {
        // Fire match start event
        EventManager.TriggerEvent("StartMatch");

        totalPlayers = playerMap.Count;

        // DEBUG
        foreach (KeyValuePair<int, Player> player in playerMap)
        {
            // do something with entry.Value or entry.Key
            Debug.Log("PlayerMap StartMatch Checking local playerMap playerID : " + player.Key + " Player.GetID() " + player.Value.GetID() + " teamID " + player.Value.teamID);
        }
    }

    [PunRPC]
    public void StartRound()
    {
        // Fire new round event
        EventManager.TriggerEvent("StartRound");
    }

    public void KillNetworkedPlayer(int playerID)
    {
        // Kill local player
        KillPlayer(playerID);

        // Make sure the rest of the clients did the same
        photonView.RPC("KillPlayerRPC", RpcTarget.Others, playerID);

        if (PhotonNetwork.IsMasterClient)
            CheckRoundEnd();
        else
            photonView.RPC("CheckRoundEndRPC", RpcTarget.MasterClient);
    }

    [PunRPC]
    void KillPlayerRPC(int playerID)
    {
        if (playerMap[playerID] != null)
        {
            playerMap[playerID].HandlePlayerDeath();

            // Add the score to the team that has killed the player with @playerID
            Debug.Log("GameManager KillPlayerRPC score. Player has been killed " + playerID);
        }
    }

    [PunRPC]
    void CheckRoundEndRPC()
    {
        Debug.Log("GameManager KillPlayerRPC We are in master client");
        CheckRoundEnd();
    }

    void KillPlayer(int playerID)
    {
        if (playerMap[playerID] != null)
        {
            playerMap[playerID].HandlePlayerDeath();

            // Add the score to the team that has killed the player with @playerID
            Debug.Log("GameManager KillPlayerRPC score. Player has been killed " + playerID);
        }
    }

    void CheckRoundEnd()
    {
        bool playerTeam1Alive = false;
        bool playerTeam2Alive = false;
        
        foreach (Player player in playerMap.Values)
        {
            Debug.Log("GameManager CheckRoundEnd Checking player " + player.GetID() + " team " + player.teamID);

            if (player.IsAlive())
            {
                if (player.teamID == 1)
                    playerTeam1Alive = true;
                else if(player.teamID == 2)
                    playerTeam2Alive = true;

                Debug.Log("GameManager CheckRoundEnd Player is alive " + player.GetID() + " from team " + player.teamID);
            }
        }

        // Players from both teams are alive so continue
        if (playerTeam1Alive && playerTeam2Alive)
        {
            Debug.Log("GameManager CheckRoundEnd players from both teams alive");
            return;
        }
        // Team 1 WON
        else if(playerTeam1Alive)
        {
            Debug.Log("GameManager CheckRoundEnd Team 1 won");
            match.FinishRound(1);
        }
        // Team 2 WON
        else if (playerTeam2Alive)
        {
            Debug.Log("GameManager CheckRoundEnd Team 2 won");
            match.FinishRound(2);
        }
    }

    [PunRPC]
    public void KillAndRespawnPlayerRPC(float respawnTimer, int playerID)
    {
        if(playerMap[playerID] != null)
        {
            playerMap[playerID].HandlePlayerDeath();
            StartCoroutine(SpawnPlayerWithDelay(respawnTimer, playerID));

            // Add the score to the team that has killed the player with @playerID
            Debug.Log("GameManager KillAndRespawnPlayerRPC score. Player has been killed " + playerID);
        }
    }

    IEnumerator SpawnPlayerWithDelay(float respawnTimer, int playerID)
    {
        // Wait respawntimer out
        yield return new WaitForSeconds(respawnTimer);

        // "Respawn" player by activating the player's GO and reset health and mana
        // The player will take care of doing this
        playerMap[playerID].Activate();
    }

    public void AddPlayerOverNetwork(int playerID)
    {
        int teamID = GetTeamToAssign();

        AddPlayer(playerID, teamID);
        photonView.RPC("AddPlayerRPC", RpcTarget.OthersBuffered, playerID, teamID);
    }

    [PunRPC]
    void AddPlayerRPC(int playerID, int teamID)
    {
        Debug.Log("GameManager AddPlayerRPC " + playerID);

        AddPlayer(playerID, teamID);
    }

    void AddPlayer(int playerID, int teamID)
    {
        Player player = GameObject.Find("Player" + playerID).GetComponent<Player>();

        player.SetTeamSpecificData(teamID);
        playerMap.Add(playerID, player);
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

        if(teamID == 0)
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
        photonView.RPC("ActivatePlayerShieldRPC", RpcTarget.Others, armor, playerID);
    }

    [PunRPC]
    void ActivatePlayerShieldRPC(int armor, int playerID)
    {
        Debug.Log("GameManager ActivatePlayerShieldRPC with " + armor + " armor for player " + playerID);
        if (playerMap[playerID] != null)
        {
            playerMap[playerID].ActivateShield(armor);
        }
    }

    public void DeactivatePlayerShield(int playerID)
    {
        photonView.RPC("DeactivatePlayerShieldRPC", RpcTarget.Others, playerID);
    }

    [PunRPC]
    void DeactivatePlayerShieldRPC(int playerID)
    {
        Debug.Log("GameManager DeactivatePlayerShieldRPC for player " + playerID);
        if (playerMap[playerID] != null)
        {
            playerMap[playerID].DeactivateShield();
        }
    }

    public void RemoveNetworkedPlayer(int playerID)
    {
        RemovePlayer(playerID);
        photonView.RPC("RemovePlayerRPC", RpcTarget.Others, playerID);
    }

    [PunRPC]
    void RemovePlayerRPC(int playerID)
    {
        RemovePlayer(playerID);
    }

    void RemovePlayer(int playerID)
    {
        if (playerMap.ContainsKey(playerID))
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
