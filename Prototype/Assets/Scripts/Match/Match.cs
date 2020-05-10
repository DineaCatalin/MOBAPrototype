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

    public const int REDRAFT_ROUND_FACTOR = 3; // Redraft will be triggered once every REDRAFT_ROUND_FACTOR

    public int team1Rounds;
    public int team2Rounds;

    int totalRoundsPlayed;

    Dictionary<int, MatchPlayer> matchPlayers;

    PhotonView photonView;

    void Awake()
    {
        team1Rounds = 0;
        team2Rounds = 0;
        totalRoundsPlayed = 0;

        matchPlayers = new Dictionary<int, MatchPlayer>();

        activeMatch = this;

        photonView = GetComponent<PhotonView>();

        EventManager.StartListening(GameEvent.EndRound, new Action(SyncScore));
    }

    public Dictionary<int, MatchPlayer> GetMatchPlayers()
    {
        return matchPlayers;
    }

    public void FinishRound(int winningTeamID)
    {
        totalRoundsPlayed++;

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
            Debug.LogError("Match FinishRoundNoTimer winning team is not 1 or 2");
        }

        PhotonNetwork.CurrentRoom.SetCustomProperties(roomProperties);

        AdvanceMatchState();
    }

    void AdvanceMatchState()
    {
        if(totalRoundsPlayed % REDRAFT_ROUND_FACTOR == 0)
        {
            Debug.Log("Match FinishRound Start Redraft");
            // Add logic for planet here
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
        Debug.LogError("Match GetCurrentWinnerTeamID winner is " + (team1Rounds > team2Rounds ? TEAM_1_ID : TEAM_2_ID));
        return team1Rounds > team2Rounds ? TEAM_1_ID : TEAM_2_ID;
    }

    void SyncRounds()
    {
        team1Rounds = (int)PhotonNetwork.CurrentRoom.CustomProperties["Team1Rounds"];
        team2Rounds = (int)PhotonNetwork.CurrentRoom.CustomProperties["Team2Rounds"];
        Debug.LogError("Match SyncRounds Team1Rounds " + team1Rounds + " Team2Rounds " + team2Rounds);
    }
}
