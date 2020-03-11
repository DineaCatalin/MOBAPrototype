using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MatchClock : MonoBehaviour
{
    public int Minutes = 3;
    public int Seconds = 0;

    int InitialMinutes;
    int InitialSeconds;

    public TextMeshProUGUI text;
    private float timeLeft;

    bool isClockRunning;

    private void Awake()
    {
        StopClock();

        InitialMinutes = Minutes;
        InitialSeconds = Seconds;

        EventManager.StartListening("StartMatch", new System.Action(OnMatchStart));
        EventManager.StartListening("StartRound", new System.Action(OnMatchStart));

        // TODO: Load match time from configuration
    }

    private void FixedUpdate()
    {
        if (timeLeft > 0f && isClockRunning)
        {
            // Update countdown clock
            timeLeft -= Time.deltaTime;
            Minutes = GetLeftMinutes();
            Seconds = GetLeftSeconds();

            // Show current clock
            if (timeLeft > 0f)
            {
                text.text = Minutes + ":" + Seconds.ToString("00");
            }
            else
            {
                // The countdown clock has finished
                text.text = "0:00";
            }
        }
    }

    void OnMatchStart()
    {
        ResetClock();
    }

    void ResetClock()
    {
        isClockRunning = true;
        timeLeft = GetInitialTime();
    }

    public void StopClock()
    {
        isClockRunning = false;
    }

    private float GetInitialTime()
    {
        return InitialMinutes * 60f + InitialSeconds;
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
