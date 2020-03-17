using System;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class MatchClock : MonoBehaviour
{
    public int Minutes = 3;
    public int Seconds = 1;

    int InitialMinutes;
    int InitialSeconds;
    int DrawSeconds;

    public TextMeshProUGUI text;
    private float timeLeft;

    bool isClockRunning;

    PhotonView photonView;

    private void Awake()
    {
        StopClock();

        InitialMinutes = Minutes;
        InitialSeconds = Seconds;

        photonView = GetComponent<PhotonView>();

        EventManager.StartListening("StartMatch", new Action(OnMatchStart));
        EventManager.StartListening("StartRound", new Action(OnMatchStart));
        EventManager.StartListening("RoundDraw", new Action(OnRoundDraw));

        // TODO: Load match time from configuration
        DrawSeconds = 16;
    }

    private void FixedUpdate()
    {
        Debug.Log("MatchClock FixedUpdate timeLeft " + timeLeft + " isClockRunning " + isClockRunning);
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

                // Send round ended event and stop the clock
                Debug.Log("MatchClock time is over triggering round end");
                StopClock();
                EventManager.TriggerEvent("RoundEnd");
            }
        }
    }

    void OnMatchStart()
    {
        ResetClock();
    }

    void OnRoundDraw()
    {
        Debug.Log("MatchClock OnRoundDraw");
        isClockRunning = true;
        timeLeft = DrawSeconds;
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
