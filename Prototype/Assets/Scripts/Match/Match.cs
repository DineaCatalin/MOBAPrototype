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

    // Lets keep track of the teams scores
    public int team1Score;
    public int team2Score;

    int maxRoundsForTeam;   // How many rounds a team needs to win the mactch

    float lastSyncTime;
    float timeFromLastExecution;

    // we will use the constructor to 
    public Match()
    {
        lastSyncTime = 0f;
        team1Rounds = 0;
        team2Rounds = 0;
        team1Score = 0;
        team2Score = 0;
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

            GameUI.Instance.SetTeamRounds(team1Rounds, TEAM_1_ID);
            Debug.Log("Match FinishRoundNoTimer Team 1 rounds won " + team1Rounds);
        }
        else if (winningTeamID == TEAM_2_ID)
        {
            team2Rounds = (int)PhotonNetwork.CurrentRoom.CustomProperties["Team2Rounds"];
            team2Rounds++;
            roomProperties.Add("Team2Rounds", team2Rounds);
            PhotonNetwork.CurrentRoom.SetCustomProperties(roomProperties);

            GameUI.Instance.SetTeamRounds(team2Rounds, TEAM_2_ID);
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



    // Use these when you want to use rounds with timer where players respawn during a round

    // Will increase of the other team then teamIDKilledPlayer
    public void IncreaseTeamScore(int teamIDKilledPlayer)
    {
        Debug.Log("Match IncreaseTeamScore Player killed from team " + teamIDKilledPlayer);

        if (teamIDKilledPlayer == TEAM_1_ID)
        {
            team2Score++;
            GameUI.Instance.SetTeamScore(team2Score, TEAM_2_ID);
            Debug.Log("Team 2 score is " + team2Score);
        }
        else if (teamIDKilledPlayer == TEAM_2_ID)
        {
            team1Score++;
            GameUI.Instance.SetTeamScore(team1Score, TEAM_1_ID);
            Debug.Log("Team 1 score is " + team1Score);
        }
    }

    // This can be called if you want to use the variant where rounds have
    // a time limit and the team with the biggest score wins
    public void FinishTimerRound()
    {
        // Team 1 WON
        if (team1Score > team2Score)
        {
            Debug.Log("Match FinishRound team 1 won round");
            team1Rounds++;
            GameUI.Instance.SetTeamRounds(team1Rounds, TEAM_1_ID);
        }

        // Team 2 WON
        else if (team2Score > team1Score)
        {
            Debug.Log("Match FinishRound team 2 won round");
            team2Rounds++;
            GameUI.Instance.SetTeamRounds(team2Rounds, TEAM_2_ID);
        }

        // We have a draw so extend the round
        else if(team1Score == team2Score)
        {
            Debug.Log("Match FinishRound Draw");
            EventManager.TriggerEvent("RoundDraw");
            return;
        }

        CheckMatchFinished();
    }

    void CheckMatchFinished()
    {
        if(team1Score >= maxRoundsForTeam)
        {
            Debug.Log("Match FinishRound Team1 won ");
            GameManager.Instance.EndMatch(TEAM_1_ID);
        }
        else if(team1Score >= maxRoundsForTeam)
        {
            Debug.Log("Match FinishRound Team2 won ");
            GameManager.Instance.EndMatch(TEAM_2_ID);
        }
        else   // Go to next round
        {
            //EventManager.TriggerEvent("StartRound");
        }
    }
}
