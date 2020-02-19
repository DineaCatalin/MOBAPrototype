using UnityEngine;
using System.Collections;

public class Match
{
    public MatchTeam team1;
    public MatchTeam team2;

    void AssignPlayer(Player player)
    {
        if (!team1.SetPlayer(player))
            team2.SetPlayer(player);
    }
}
