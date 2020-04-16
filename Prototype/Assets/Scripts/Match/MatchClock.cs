using System;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class MatchClock : MonoBehaviour
{
    public int Minutes = 0;
    public int Seconds = 15;

    int InitialMinutes;
    int InitialSeconds;

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

        EventManager.StartListening(GameEvent.StartRound, new Action(ResetClock));
        //EventManager.StartListening(GameEvent.StartMatch, new System.Action(ResetClock));
        EventManager.StartListening(GameEvent.ItemPickedUp, new Action(ResetClock));

        EventManager.StartListening(GameEvent.StartRedraft, new Action(StopClock));
        EventManager.StartListening(GameEvent.EndRound, new Action(StopClock));
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

            Debug.Log("MatchClock FixedUpdate time is " + Seconds);

            // Show current clock
            if (timeLeft > 0f)
            {
                //text.text = Minutes + ":" + Seconds.ToString("00");
                if(Seconds >= 10)
                    text.text = (Seconds+1).ToString("00");
                else
                    text.text = (Seconds+1).ToString("0");

            }
            else
            {
                // The countdown clock has finished
                text.text = "";

                // Send round ended event and stop the clock
                Debug.Log("MatchClock time is over triggering round end");
                StopClock();
                EventManager.TriggerEvent(GameEvent.SpawnItem);
            }
        }
    }

    void ResetClock()
    {
        Debug.Log("MatchClock ResetClock");
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
