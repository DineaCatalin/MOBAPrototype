using UnityEngine;
using System.Collections;

public class Match
{
    public MatchTeam team1;
    public MatchTeam team2;

    // Lets keep track of the teams scores
    public int team1Score;
    public int team2Score;

    // We will use this to Assign each connected player to a different team
    // 1st to team 1, 2nd to team 2, 3rd to team 1 and 4th to team2
    int playerCounter;

    public Match()
    {
        team1 = new MatchTeam(1);
        team2 = new MatchTeam(2); 
    }

    // Will increase of the other team then teamIDKilledPlayer
    public int IncreaseTeamScore(int teamIDKilledPlayer)
    {
        if (teamIDKilledPlayer == 1)
        {
            team2Score++;
            Debug.Log("Team 2 score is " + team2Score);
            // Change UI
            return team2Score;
        }
        else if (teamIDKilledPlayer == 2)
        {
            team1Score++;
            Debug.Log("Team 1 score is " + team1Score);
            // Change UI
            return team1Score;
        }

        return 0;
    }

    public int AssignPlayer(Player player)
    {
        if (team1.SetPlayer(player))
        {
            return 1;
        }
        else if(team2.SetPlayer(player))
        {
            return 2;
        }

        return 0;
    }

    public void AssignPlayer(Player player, int teamID)
    {
        if (teamID == 1)
        {
            team1.SetPlayer(player);
        }
        else if (teamID == 2)
        {
            team2.SetPlayer(player);
        }
    }
}
