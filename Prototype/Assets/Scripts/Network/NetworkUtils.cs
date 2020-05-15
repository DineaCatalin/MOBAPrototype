using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class NetworkUtils : MonoBehaviourPun
{
    public const float PLAYER_SPAWN_DELAY = 1f;
    float lag;

    public static NetworkUtils SharedInstance;

    void Awake()
    {
        SharedInstance = this;
    }

    public float NonLocalPlayerSpawnDelay(double SentServerTime)
    {
        lag = Mathf.Abs((float)(PhotonNetwork.Time - SentServerTime));
        Debug.Log("NetworkUtils NonLocalPlayerSpawnDelay lag " + lag + " PLAYER_SPAWN_DELAY - lag " + (PLAYER_SPAWN_DELAY - lag));
        return PLAYER_SPAWN_DELAY - lag;
    }
}
