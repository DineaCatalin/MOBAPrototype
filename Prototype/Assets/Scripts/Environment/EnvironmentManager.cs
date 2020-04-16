﻿using UnityEngine;
using System.Collections;
using Photon.Pun;

public class EnvironmentManager : MonoBehaviour
{
    // This will be triggered by 1 of the abilities
    [SerializeField] GameObject dustDusk;

    SpriteRenderer dustDuskRenderer;
    Transform dustDuskTransform;
    DustDuskFadeTween dustDuskFadeTween;


    PhotonView photonView;

    [SerializeField] float team1SpawnPosX = -11;
    [SerializeField] float team2SpawnPosX = 11;

    // This will help us position the walls and area limiters in the same pla
    public Vector2 environmentSize;

    public static EnvironmentManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        photonView = GetComponent<PhotonView>();

        // Let the master client decide what the environment size is based on it's screen size
        // and then update the rest of the clients with this value
        if (PhotonNetwork.IsMasterClient)
        {
            environmentSize = Utils.Instance.CameraScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
            photonView.RPC("SetEnvironmentSize", RpcTarget.OthersBuffered, environmentSize.x, environmentSize.y);
        }
    }

    private void Start()
    {
        dustDuskRenderer = dustDusk.GetComponent<SpriteRenderer>();
        dustDuskTransform = dustDusk.GetComponent<Transform>();
        dustDuskFadeTween = dustDusk.GetComponent<DustDuskFadeTween>();

        ResizeToScreenSize();
    }

    [PunRPC]
    void SetEnvironmentSize(float x, float y)
    {
        environmentSize = new Vector2(x, y);
    }

    public void TriggerDustDusk(int duration, int teamID)
    {
        photonView.RPC("ActivateDustDusk", RpcTarget.Others, teamID);

        //StartCoroutine(HideDuskDust(duration));
    }

    [PunRPC]
    void ActivateDustDusk(int casterTeamID)
    {
        // We can add a fade here later
        if (Player.localTeamID != casterTeamID)
            dustDuskFadeTween.Fade();
    }

    void ResizeToScreenSize()
    {

        Debug.Log("Resizing");
        transform.localScale = Vector3.one;

        float width = dustDuskRenderer.sprite.bounds.size.x;
        float height = dustDuskRenderer.sprite.bounds.size.y;

        float worldScreenHeight = Camera.main.orthographicSize * 2.0f;
        float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

        dustDuskTransform.localScale = new Vector3(worldScreenWidth / width, worldScreenHeight / height, 1);
    }

    public Vector3 GetPlayerSpawnPoint(int teamID)
    {
        float spawnY = Random.Range
                (Utils.Instance.CameraScreenToWorldPoint(new Vector2(0, 0)).y, Utils.Instance.CameraScreenToWorldPoint(new Vector2(0, Screen.height)).y);

        spawnY /= 2;

        if(teamID == 1)
        {
            return new Vector3(team1SpawnPosX, spawnY, 0);
        }
        else
        {
            return new Vector3(team2SpawnPosX, spawnY, 0);
        }
    }
}
