using UnityEngine;
using Photon.Pun;

public class PUN2_PlayerSync : MonoBehaviourPun, IPunObservable
{
    // The GO that contains the player will take care of doing the sync
    public GameObject player2Sync;

    Player player;

    //List of the scripts that should only be active for the local player (ex. PlayerController, MouseLook etc.)
    public MonoBehaviour[] localScripts;

    //List of the GameObjects that should only be active for the local player (ex. Camera, AudioListener etc.)
    public GameObject[] localObjects;

    //Values that will be synced over network
    Rigidbody2D playerRigidbody;
    Transform playerTransform;

    Vector2 networkPosition;
    float networkRotation;
    Vector2 velocity;
    float angularVelocity;

    //Lag compensation
    float currentTime = 0;
    double currentPacketTime = 0;
    double lastPacketTime = 0;
    Vector3 positionAtLastPacket = Vector3.zero;
    Quaternion rotationAtLastPacket = Quaternion.identity;

    // Health and mana
    int health;
    int lastHealth;
    int mana;
    int lastMana;

    void Awake()
    {
        playerRigidbody = player2Sync.GetComponent<Rigidbody2D>();
        player = player2Sync.GetComponent<Player>();
        playerTransform = player2Sync.transform;

        health = player.GetHealth();
        lastHealth = health;
        mana = player.GetMana();
        lastMana = mana;

        if (!photonView.IsMine)
        {
            //Player is Remote, deactivate the scripts and object that should only be enabled for the local player
            for (int i = 0; i < localScripts.Length; i++)
            {
                localScripts[i].enabled = false;
            }
            for (int i = 0; i < localObjects.Length; i++)
            {
                localObjects[i].SetActive(false);
            }

            player.isNetworkActive = false;
        }
        else
        {
            player.isNetworkActive = true;
        }

        player.nickName.text = photonView.Owner.NickName;
    }

    void Update()
    {
        if (!photonView.IsMine)
        {
            if(health != lastHealth)
            {
                player.SetHealth(health);
                lastHealth = health;
            }

            if (mana != lastMana)
            {
                player.SetMana(mana);
                lastMana = mana;
            }
        }
    }

    void FixedUpdate()
    {
        if (!photonView.IsMine)
        {
            playerRigidbody.position = Vector3.MoveTowards(playerRigidbody.position, networkPosition, Time.fixedDeltaTime);
            //playerRigidbody.MovePosition(Vector3.MoveTowards(playerRigidbody.position, networkPosition, Time.fixedDeltaTime));


            // Don't do rotation
            //playerRigidbody.rotation = Quaternion.RotateTowards(playerRigidbody.rotation, networkRotation, Time.fixedDeltaTime * 100.0f);
            //playerRigidbody.MoveRotation(networkRotation);
        }
    }

    float lag;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // Rigidbody
            stream.SendNext(playerRigidbody.position);
            stream.SendNext(playerRigidbody.rotation);
            stream.SendNext(playerRigidbody.velocity);

            //stream.SendNext(playerTransform.position);

            // Health and mana
            stream.SendNext(player.GetStats().health);
            stream.SendNext(player.GetStats().mana);
        }
        else
        {
            networkPosition = (Vector2)stream.ReceiveNext();
            networkRotation = (float)stream.ReceiveNext();
            playerRigidbody.velocity = (Vector2)stream.ReceiveNext();

            //playerTransform.position = (Vector3)stream.ReceiveNext();

            // Lag compensation
            lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));
            //playerRigidbody.position += playerRigidbody.velocity * lag;
            networkPosition += (playerRigidbody.velocity * lag);

            // Health and mana
            health = (int)stream.ReceiveNext();
            mana = (int)stream.ReceiveNext();
        }
    }
}


