using UnityEngine;
using System.Collections;
using System;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class GameManager : MonoBehaviour
{
    // TODO: Load this from a config later
    // PLS don't leave it like this
    public static float RESPAWN_COOLDOWN = 3f;
    public static float MAX_PLAYERS = 4f; 

    public static float TIME_BETWEEN_ROUNDS = 1.5f;

    public const float ACTIVATE_PLAYER_DELAY = 0.75f;

    public static GameManager Instance;

    PhotonView photonView;

    // Holds team and score
    Match match;

    // Total players in this game
    int totalPlayers;
    int playersReady;

    private void Awake()
    {
        Instance = this;
        photonView = GetComponent<PhotonView>();
        
        playersReady = 0;

        EventManager.StartListening(GameEvent.EndRound, new Action(OnRoundEnd));
        EventManager.StartListening(GameEvent.PlanetStateAdvance, new Action(AdvancePlanetState)); 
        EventManager.StartListening(GameEvent.EndRedraft, new Action(OnRedraftEnd));
        EventManager.StartListening(GameEvent.EndMatch, new Action(EndMatch));

        match = GetComponent<Match>();
        Hashtable roomProperties = new Hashtable();

        if (PhotonNetwork.IsMasterClient)
        {
            // Assign the new value to the room properties
            roomProperties.Add("CurrentWinnerTeamID", 0);
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
            if (Input.GetKeyDown(KeyCode.Return) && !matchStarted)
            {
                StartMatch();

                photonView.RPC("StartMatchRPC", RpcTarget.Others);
                matchStarted = true;

                // Close the room so that other players can't connect
                PhotonNetwork.CurrentRoom.IsOpen = false;
            }
        }
    }

    [PunRPC]
    void StartMatchRPC()
    {
        StartMatch();
    }

    void StartMatch()
    {
        // Fire match start event
        EventManager.TriggerEvent(GameEvent.StartMatch);

        //Debug.Log("GameManager StartMatch totalPlayers " + totalPlayers);
        totalPlayers = PlayerManager.Instance.GetPlayerCount();

        Debug.Log("GameManager StartMatch totalPlayers " + totalPlayers);

        Debug.Log("GameManager StartMatch StartRound");
        EventManager.TriggerEvent(GameEvent.StartRound);
    }

    void OnRoundEnd()
    {
        Debug.Log("GameManager OnRoundEnd");
        EndRound();

        if (PhotonNetwork.IsMasterClient)
            photonView.RPC("RoundEndRPC", RpcTarget.Others);
    }

    [PunRPC]
    public void RoundEndRPC()
    {
        EventManager.TriggerEvent(GameEvent.EndRound);
    }

    void EndRound()
    {
        StartCoroutine("StartRoundWithDelay");
    }

    IEnumerator StartRoundWithDelay()
    {
        yield return new WaitForSeconds(TIME_BETWEEN_ROUNDS);

        Debug.Log("GameManager StartRoundWithDelay Starting round");
        StartRound();
    }

    void StartRound()
    {
        // Fire new round event
        Debug.Log("GameManager StartRound");
        EventManager.TriggerEvent(GameEvent.StartRound);

        playersReady = 0;
    }

    //// Handle match end logic
    public void EndMatch()
    {
        Debug.Log("Match ENDED!");
    }

    public void CheckRoundEndMasterClient()
    {
        if (PhotonNetwork.IsMasterClient)
            CheckRoundEnd();
        else
            photonView.RPC("CheckRoundEndRPC", RpcTarget.MasterClient);
    }

    void CheckRoundEnd()
    {
        bool playerTeam1Alive = false;
        bool playerTeam2Alive = false;

        foreach (Player player in PlayerManager.Instance.GetPlayerMap().Values)
        {
            Debug.Log("GameManager CheckRoundEnd Checking player " + player.GetID() + " team " + player.teamID);

            if (player.IsAlive())
            {
                if (player.teamID == Match.TEAM_1_ID)
                    playerTeam1Alive = true;
                else if (player.teamID == Match.TEAM_2_ID)
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
        else if (playerTeam1Alive)
        {
            Debug.Log("GameManager CheckRoundEnd Team 1 won");
            match.FinishRound(Match.TEAM_1_ID);
        }
        // Team 2 WON
        else if (playerTeam2Alive)
        {
            Debug.Log("GameManager CheckRoundEnd Team 2 won");
            match.FinishRound(Match.TEAM_2_ID);
        }
    }

    [PunRPC]
    void CheckRoundEndRPC()
    {
        Debug.Log("GameManager KillPlayerRPC We are in master client");
        CheckRoundEnd();
    }

    [PunRPC]
    void CheckRedraftFinishedRPC()
    {
        CheckRedraftFinished();
    }

    void CheckRedraftFinished()
    {
        playersReady++;

        if (playersReady >= totalPlayers)
        {
            Debug.Log("GameManager CheckRedraftFinished OnRoundEnd playersReady " + playersReady + " totalPlayers " + totalPlayers);
            OnRoundEnd();
        }
    }

    void AdvancePlanetState()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("AdvancePlanetStateRPC", RpcTarget.Others);
        }
    }

    [PunRPC]
    void AdvancePlanetStateRPC()
    {
        EventManager.TriggerEvent(GameEvent.PlanetStateAdvance);
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
}
