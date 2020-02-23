using UnityEngine;
using System.Collections;

// Wanted to name this team but it created some conflicts so this will have to do
public class MatchTeam
{
    public Player player1;
    public Player player2;

    int teamID;

    public MatchTeam(int teamID)
    {
        this.teamID = teamID;
    }

    int score;
    public int Score
    {
        get { return score;  }
        set { score = value; }
    }

    public bool SetPlayer(Player player)
    {
        // If 1st player is not set set it
        if(player1 == null)
        {
            player1 = player;
            
            player.SetTeamSpecificData(teamID);
            return true;
        }

        // If 2nd player is not set set it
        if (player2 == null)
        {
            player2 = player;
            player.SetTeamSpecificData(teamID);
            return true;
        }

        // Team is full so can't set another playerID
        return false;
    }

    // Set a player to a specific team
    public bool SetPlayer(Player player, int teamID)
    {
        if (teamID == 1)
        {
            player1 = player;
            player.SetTeamSpecificData(teamID);
            return true;
        } 
        else if (teamID == 2)
        {
            player2 = player;
            player.SetTeamSpecificData(teamID);
            return true;
        }

        return false;
    }

}
