using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreBoard : MonoBehaviour
{
    public MatchPlayerUI[] playerScoreUIs;
    Dictionary<int, MatchPlayerUI> playerScoreMap;

    public static ScoreBoard Instance;

    // Start is called before the first frame update
    void Awake()
    {
        playerScoreMap = new Dictionary<int, MatchPlayerUI>();
        Instance = this;
        EventManager.StartListening(GameEvent.StartMatch, new System.Action(SetComponents));
    }

    void SetComponents()
    {
        int index = 0;

        foreach (MatchPlayer matchPlayer in Match.activeMatch.GetMatchPlayers().Values)
        {
            playerScoreUIs[index].nameText.text = matchPlayer.nickName;
            playerScoreMap.Add(matchPlayer.ID, playerScoreUIs[index]);
            index++;
        }

        playerScoreUIs = null;
    }

    public void SetKillScore(int killerPlayerID, int score)
    {
        playerScoreMap[killerPlayerID].SetKills(score);
    }

    public void SetDeathScore(int killedPlayerID, int score)
    {
        playerScoreMap[killedPlayerID].SetDeaths(score);
    }
}
