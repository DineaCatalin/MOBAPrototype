using UnityEngine;
using System;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Photon.Pun;
using System.Collections.Generic;

public class Match : MonoBehaviour
{
    public static Match activeMatch;

    public static int TEAM_1_ID = 1;
    public static int TEAM_2_ID = 2;

    public const int PHASE_ROUNDS = 3; // Redraft will be triggered once every REDRAFT_ROUND_FACTOR

    public int team1Rounds;
    public int team2Rounds;

    int totalRoundsPlayed;
    int roundsCurrentPhase;

    Dictionary<int, MatchPlayer> matchPlayers;

    PhotonView photonView;

    void Awake()
    {
        team1Rounds = 0;
        team2Rounds = 0;
        totalRoundsPlayed = 0;
        roundsCurrentPhase = 0;

        matchPlayers = new Dictionary<int, MatchPlayer>();

        activeMatch = this;

        photonView = GetComponent<PhotonView>();

        EventManager.StartListening(GameEvent.EndRedraft, new Action(ResetRounds));

        EventManager.StartListening(GameEvent.EndRound, new Action(SyncScore));
        EventManager.StartListening(GameEvent.PlanetStateAdvance, new Action(SyncScore));
    }

    public Dictionary<int, MatchPlayer> GetMatchPlayers()
    {
        return matchPlayers;
    }

    public void FinishRound(int winningTeamID)
    {
        totalRoundsPlayed++;
        roundsCurrentPhase++;

        Hashtable roomProperties = new Hashtable();

        if (winningTeamID == TEAM_1_ID)
        {
            team1Rounds = (int)PhotonNetwork.CurrentRoom.CustomProperties["Team1Rounds"];
            team1Rounds++;
            roomProperties.Add("Team1Rounds", team1Rounds);
            PhotonNetwork.CurrentRoom.SetCustomProperties(roomProperties);

            Debug.Log("Match FinishRoundNoTimer Team 1 rounds won " + team1Rounds);
        }
        else if (winningTeamID == TEAM_2_ID)
        {
            team2Rounds = (int)PhotonNetwork.CurrentRoom.CustomProperties["Team2Rounds"];
            team2Rounds++;
            roomProperties.Add("Team2Rounds", team2Rounds);
            PhotonNetwork.CurrentRoom.SetCustomProperties(roomProperties);

            Debug.Log("Match FinishRoundNoTimer Team 2 rounds won " + team2Rounds);
        }
        else
        {
            Debug.Log("Match FinishRoundNoTimer winning team is not 1 or 2");
        }

        PhotonNetwork.CurrentRoom.SetCustomProperties(roomProperties);

        AdvanceMatchState();
    }

    void AdvanceMatchState()
    {
        if(PhaseEnded())
        {
            Debug.Log("Match AdvanceMatchState Triggering planet transition event");
            EventManager.TriggerEvent(GameEvent.PlanetStateAdvance);
        }
        else
        {
            Debug.Log("Match FinishRound Start Redraft");
            EventManager.TriggerEvent(GameEvent.EndRound);
        }

        Debug.Log("Match FinishRound ");
    }

    public void SyncScore()
    {
        SyncRounds();

        SyncScoreUI();
    }

    bool PhaseEnded()
    {
        Debug.Log("Match PhaseEnded Team1Rounds " + team1Rounds + " Team2Rounds " + team2Rounds + " (PHASE_ROUNDS / 2) " + (PHASE_ROUNDS / 2));
        return team1Rounds > (PHASE_ROUNDS / 2) || team2Rounds > (PHASE_ROUNDS / 2);
    }

    void SyncScoreUI()
    {
        GameUI.Instance.SetTeamRounds(team1Rounds, TEAM_1_ID);
        GameUI.Instance.SetTeamRounds(team2Rounds, TEAM_2_ID);
    }

    public void AddMatchPlayer(string nickName,int playerID, int playerTeamID)
    {
        matchPlayers.Add(playerID, new MatchPlayer(nickName, playerID, playerTeamID));
    }

    public void AddKillScore(int killerPlayerID, int killedPlayerID)
    {
        matchPlayers[killerPlayerID].AddKill();
        matchPlayers[killedPlayerID].AddDeath();

        ScoreBoard.Instance.SetKillScore(killerPlayerID, matchPlayers[killerPlayerID].kills);
        ScoreBoard.Instance.SetDeathScore(killedPlayerID, matchPlayers[killedPlayerID].deaths);
    }

    public int GetCurrentWinnerTeamID()
    {
        SyncRounds();
        Debug.Log("Match GetCurrentWinnerTeamID winner is " + (team1Rounds > team2Rounds ? TEAM_1_ID : TEAM_2_ID));
        return team1Rounds > team2Rounds ? TEAM_1_ID : TEAM_2_ID;
    }

    void SyncRounds()
    {
        team1Rounds = (int)PhotonNetwork.CurrentRoom.CustomProperties["Team1Rounds"];
        team2Rounds = (int)PhotonNetwork.CurrentRoom.CustomProperties["Team2Rounds"];
        Debug.Log("Match SyncRounds Team1Rounds " + team1Rounds + " Team2Rounds " + team2Rounds);
    }

    void ResetRounds()
    {
        roundsCurrentPhase = 0;

        team1Rounds = 0;
        team2Rounds = 0;

        Hashtable roomProperties = new Hashtable();
        roomProperties.Add("Team1Rounds", 0);
        roomProperties.Add("Team2Rounds", 0);
        PhotonNetwork.CurrentRoom.SetCustomProperties(roomProperties);

        SyncScoreUI();
    }
}
