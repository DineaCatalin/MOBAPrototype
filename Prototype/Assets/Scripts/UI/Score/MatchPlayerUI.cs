using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MatchPlayerUI : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI killsText;
    public TextMeshProUGUI deathsText;

    const string START_SCORE = "0";

    // Start is called before the first frame update
    void Awake()
    {
        killsText.text = START_SCORE;
        deathsText.text = START_SCORE;
    }

    public void SetKills(int kills)
    {
        killsText.text = kills.ToString();
    }

    public void SetDeaths(int deaths)
    {
        deathsText.text = deaths.ToString();
    }

}
