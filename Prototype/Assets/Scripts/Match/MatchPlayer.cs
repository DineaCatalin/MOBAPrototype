using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchPlayer
{
    public int ID;
    public int teamID;

    public int kills;
    public int deaths;

    public MatchPlayer(int playerID, int playerTeamID)
    {
        ID = playerID;
        teamID = playerTeamID;
    }

    public void AddDeath()
    {
        deaths++;
    }

    public void AddKill()
    {
        kills++;
    }

}
