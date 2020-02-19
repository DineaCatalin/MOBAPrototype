using UnityEngine;
using System.Collections;

// Wanted to name this team but it created some conflicts so this will have to do
public class MatchTeam
{
    public Player player1;
    public Player player2;

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
            return true;
        }

        // If 2nd player is not set set it
        if (player2 == null)
        {
            player2 = player;
            return true;
        }

        // Team is full so can't set another playerID
        return false;
    }

}
