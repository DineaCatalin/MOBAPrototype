using Photon.Pun;
using UnityEngine;

public class PUN2_PlayerSync2 : MonoBehaviourPun, IPunObservable
{
    // The GO that contains the player will take care of doing the sync
    public GameObject player2Sync;

    Player player;

    //List of the scripts that should only be active for the local player (ex. PlayerController, MouseLook etc.)
    public MonoBehaviour[] localScripts;

    //List of the GameObjects that should only be active for the local player (ex. Camera, AudioListener etc.)
    public GameObject[] localObjects;

    //Values that will be synced over network
    Rigidbody2D rb;
    Transform playerTransform;
    NonLocalPlayerMovement nonLocalMovement;

    Vector3 latestPos;
    Quaternion latestRot;
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
        rb = player2Sync.GetComponent<Rigidbody2D>();
        player = player2Sync.GetComponent<Player>();
        playerTransform = player2Sync.transform;
        nonLocalMovement = player2Sync.GetComponent<NonLocalPlayerMovement>();

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
            //Lag compensation
            double timeToReachGoal = currentPacketTime - lastPacketTime;
            currentTime += Time.deltaTime;

            //Update remote player
            //playerTransform.position = Vector3.Lerp(positionAtLastPacket, latestPos, (float)(currentTime / timeToReachGoal));
            nonLocalMovement.Move(Vector3.Lerp(positionAtLastPacket, latestPos, (float)(currentTime / timeToReachGoal)));
            playerTransform.rotation = Quaternion.Lerp(rotationAtLastPacket, latestRot, (float)(currentTime / timeToReachGoal));

            //if (health != lastHealth)
            //{
            //    player.SetHealth(health);
            //    lastHealth = health;
            //}

            //if (mana != lastMana)
            //{
            //    player.SetMana(mana);
            //    lastMana = mana;
            //}
        }
    }

    //void FixedUpdate()
    //{
    //    if (!photonView.IsMine)
    //    {
    //        rb.velocity = velocity;
    //        rb.angularVelocity = angularVelocity;
    //    }
    //}

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            //We own this player: send the others our data
            // Transform
            stream.SendNext(playerTransform.position);
            stream.SendNext(playerTransform.rotation);

            // Rigidbody
            //stream.SendNext(rb.velocity);
            //stream.SendNext(rb.angularVelocity);

            // Health and mana
            //stream.SendNext(player.GetStats().health);
            //stream.SendNext(player.GetStats().mana);
        }
        else
        {
            Debug.Log("PUN2_PlayerSync OnPhotonSerializeView Start not my player " + photonView.ViewID);
            //Network player, receive data
            latestPos = (Vector3)stream.ReceiveNext();
            latestRot = (Quaternion)stream.ReceiveNext();
            //velocity = (Vector2)stream.ReceiveNext();
            //angularVelocity = (float)stream.ReceiveNext();

            //Lag compensation
            currentTime = 0.0f;
            lastPacketTime = currentPacketTime;
            currentPacketTime = info.SentServerTime;

            Debug.Log("PUN2_PlayerSync OnPhotonSerializeView After transform check not my player " + photonView.ViewID);
            positionAtLastPacket = playerTransform.position;
            rotationAtLastPacket = playerTransform.rotation;

            // Health and mana
            //health = (int)stream.ReceiveNext();
            //mana = (int)stream.ReceiveNext();
        }
    }
}