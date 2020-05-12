using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class NetworkUtils : MonoBehaviourPun, IPunObservable
{
    public const float PLAYER_SPAWN_DELAY = 1f;
    public float lag;

    public static NetworkUtils SharedInstance;

    void Awake()
    {
        SharedInstance = this;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        Debug.Log("NetworkUtils lag : " + lag);
        lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));
        Debug.Log("NetworkUtils lag : " + lag);
    }

    public float NonLocalPlayerSpawnDelay()
    {
        Debug.LogError("NetworkUtils NonLocalPlayerSpawnDelay " + (PLAYER_SPAWN_DELAY - lag) + " lag = " + lag);
        return PLAYER_SPAWN_DELAY - lag;
    }
}
