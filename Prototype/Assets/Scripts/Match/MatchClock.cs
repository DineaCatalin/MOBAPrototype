using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MatchClock : MonoBehaviour
{
    public int Minutes = 3;
    public int Seconds = 0;

    public TextMeshProUGUI text;
    private float timeLeft;

    private void Awake()
    {
        timeLeft = GetInitialTime();

        // TODO: Load match time from configuration
    }

    private void Update()
    {
        if (timeLeft > 0f)
        {
            //  Update countdown clock
            timeLeft -= Time.deltaTime;
            Minutes = GetLeftMinutes();
            Seconds = GetLeftSeconds();

            //  Show current clock
            if (timeLeft > 0f)
            {
                text.text = Minutes + ":" + Seconds.ToString("00");
            }
            else
            {
                //  The countdown clock has finished
                text.text = "0:00";
            }
        }
    }

    private float GetInitialTime()
    {
        return Minutes * 60f + Seconds;
    }

    private int GetLeftMinutes()
    {
        return Mathf.FloorToInt(timeLeft / 60f);
    }

    private int GetLeftSeconds()
    {
        return Mathf.FloorToInt(timeLeft % 60f);
    }
}
