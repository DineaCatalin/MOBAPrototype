using UnityEngine;
using System;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Photon.Pun;

public class Match
{
    const int TEAM_1_ID = 1;
    const int TEAM_2_ID = 2;

    public int team1Rounds;
    public int team2Rounds;

    int maxRoundsForTeam;   // How many rounds a team needs to win the mactch

    // we will use the constructor to 
    public Match()
    {
        team1Rounds = 0;
        team2Rounds = 0;
        maxRoundsForTeam = GameManager.ROUNDS_TO_WIN;
    }

    public void FinishRoundNoTimer(int winningTeamID)
    {
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

        Debug.Log("Match FinishRoundNoTimer ");
        EventManager.TriggerEvent("RoundEnd");
    }

    public void SyncScore()
    {
        team1Rounds = (int)PhotonNetwork.CurrentRoom.CustomProperties["Team1Rounds"];
        team2Rounds = (int)PhotonNetwork.CurrentRoom.CustomProperties["Team2Rounds"];

        Debug.Log("Match SyncScore Team1 " + team1Rounds + " Team2 " + team2Rounds);

        GameUI.Instance.SetTeamRounds(team1Rounds, TEAM_1_ID);
        GameUI.Instance.SetTeamRounds(team2Rounds, TEAM_2_ID);
    }

}
