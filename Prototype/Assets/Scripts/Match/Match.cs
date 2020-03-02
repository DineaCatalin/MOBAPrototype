using UnityEngine;
using System.Collections;

public class Match
{
    // Lets keep track of the teams scores
    public int team1Score;
    public int team2Score;

    // Will increase of the other team then teamIDKilledPlayer
    public void IncreaseTeamScore(int teamIDKilledPlayer)
    {
        Debug.Log("Match IncreaseTeamScore Player killed from team " + teamIDKilledPlayer);

        if (teamIDKilledPlayer == 1)
        {
            team2Score++;
            GameUI.Instance.SetTeamScore(team2Score, 2);
            Debug.Log("Team 2 score is " + team2Score);
        }
        else if (teamIDKilledPlayer == 2)
        {
            team1Score++;
            GameUI.Instance.SetTeamScore(team1Score, 1);
            Debug.Log("Team 1 score is " + team1Score);
        }
    }
}
