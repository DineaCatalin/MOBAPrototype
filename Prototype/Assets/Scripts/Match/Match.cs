using UnityEngine;
using System;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Photon.Pun;
using System.Collections.Generic;

public class Match : MonoBehaviour
{
    public static int TEAM_1_ID = 1;
    public static  int TEAM_2_ID = 2;

    const int ROUNDS_TO_WIN = 9;
    const int REDRAFT_ROUND_FACTOR = 3; // Redraft will be triggered once every REDRAFT_ROUND_FACTOR

    public int team1Rounds;
    public int team2Rounds;

    int totalRoundsPlayed;

    Dictionary<int, MatchPlayer> matchPlayers;

    // we will use the constructor to 
    public Match()
    {
        team1Rounds = 0;
        team2Rounds = 0;
        totalRoundsPlayed = 0;

        matchPlayers = new Dictionary<int, MatchPlayer>();
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
        if (team1Rounds >= ROUNDS_TO_WIN)
        {
            // TEAM 1 WON
            Debug.Log("Match FinishRound Team 1 WON");
        }
        if(team2Rounds >= ROUNDS_TO_WIN)
        {
            // TEAM 2 WON
            Debug.Log("Match FinishRound Team 2 WON");
        }

        if(totalRoundsPlayed % REDRAFT_ROUND_FACTOR == 0)
        {
            Debug.Log("Match FinishRound Start Redraft");
            EventManager.TriggerEvent(GameEvent.StartRedraft);
        }
        else
        {
            Debug.Log("Match FinishRound Start Redraft");
            EventManager.TriggerEvent(GameEvent.RoundEnd);
        }

        Debug.Log("Match FinishRound ");
    }

    public void SyncScore()
    {
        team1Rounds = (int)PhotonNetwork.CurrentRoom.CustomProperties["Team1Rounds"];
        team2Rounds = (int)PhotonNetwork.CurrentRoom.CustomProperties["Team2Rounds"];

        Debug.Log("Match SyncScore Team1 " + team1Rounds + " Team2 " + team2Rounds);

        GameUI.Instance.SetTeamRounds(team1Rounds, TEAM_1_ID);
        GameUI.Instance.SetTeamRounds(team2Rounds, TEAM_2_ID);
    }

    public void AddMatchPlayer(int playerID, int playerTeamID)
    {
        matchPlayers.Add(playerID, new MatchPlayer(playerID, playerTeamID));
    }

    public void AddKillScore(int killerPlayerID, int killedPlayerID)
    {
        matchPlayers[killerPlayerID].AddKill();
        matchPlayers[killedPlayerID].AddDeath();
    }
}
