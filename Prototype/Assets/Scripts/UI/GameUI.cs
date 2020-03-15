using UnityEngine;
using System.Collections;
using TMPro;
using System;

// This will contain most of the UI elements in the game
// Except the player health and mana bars
// So cooldowns, match stats etc...
public class GameUI : MonoBehaviour
{
    public static GameUI Instance;

    // The Score means how many kills a team has in 1 round
    public TextMeshProUGUI team1Score;
    public TextMeshProUGUI team2Score;

    // The Round amount means how many rounds a team has won
    public TextMeshProUGUI team1Rounds;
    public TextMeshProUGUI team2Rounds;

    // Use this for initialization
    void Awake()
    {
        Instance = this;
    }

    public void SetTeamScore(int score, int teamID)
    {
        if(teamID == 1)
        {
            team1Score.text = score.ToString();
        }
        else if(teamID == 2)
        {
            team2Score.text = score.ToString();
        }
    }

    public void SetTeamRounds(int rounds, int teamID)
    {
        if (teamID == 1)
        {
            team1Rounds.text = rounds.ToString();
        }
        else if (teamID == 2)
        {
            team2Rounds.text = rounds.ToString();
        }
    }
}