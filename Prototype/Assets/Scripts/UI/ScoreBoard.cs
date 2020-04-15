using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreBoard : MonoBehaviour
{
    public Match match;

    public TextMeshProUGUI localPlayerName;
    public TextMeshProUGUI localPlayerScore;

    public TextMeshProUGUI teamMateName;
    public TextMeshProUGUI teamMateScore;

    public TextMeshProUGUI enemy1Name;
    public TextMeshProUGUI enemy1Score;

    public TextMeshProUGUI enemy2Name;
    public TextMeshProUGUI enemy2Score;

    // Start is called before the first frame update
    void Start()
    {
        EventManager.StartListening(GameEvent.StartMatch, new System.Action(SetNames));
    }

    void SetNames()
    {
        
    }
}
