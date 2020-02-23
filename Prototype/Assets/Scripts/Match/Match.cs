using UnityEngine;
using System.Collections;

public class Match
{
    public MatchTeam team1;
    public MatchTeam team2;

    // We will use this to Assign each connected player to a different team
    // 1st to team 1, 2nd to team 2, 3rd to team 1 and 4th to team2
    int playerCounter;

    public Match()
    {
        team1 = new MatchTeam(1);
        team2 = new MatchTeam(2); 
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

    public int AssignPlayerNoPreference(Player player)
    {
        int teamID = playerCounter % 2 + 1;
        Debug.Log("Match AssignPlayerNoPreference to team " + teamID);

        if (teamID == 1)
            team1.SetPlayer(player);
        else if (teamID == 2)
            team2.SetPlayer(player);
        
        playerCounter++;

        return teamID;
    }
}
