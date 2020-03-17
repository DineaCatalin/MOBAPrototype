using UnityEngine;
using System;

public class Match
{
    const int TEAM_1_ID = 1;
    const int TEAM_2_ID = 2;

    public int team1Rounds;
    public int team2Rounds;

    // Lets keep track of the teams scores
    public int team1Score;
    public int team2Score;

    int currentRound;
    int maxRoundsForTeam;   // How many rounds a team needs to win the mactch

    // we will use the constructor to 
    public Match()
    {
        team1Rounds = 0;
        team2Rounds = 0;
        team1Score = 0;
        team2Score = 0;
        maxRoundsForTeam = GameManager.ROUNDS_TO_WIN;

        EventManager.StartListening("RoundEnd", new Action(FinishRound));
    }

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

    public void FinishRound()
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
            EventManager.TriggerEvent("StartRound");
        }
    }
}
