using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;

public class PUN2_PlayerSync : MonoBehaviourPun, IPunObservable
{
    // The GO that contains the player will take care of doing the sync
    public GameObject player2Sync;

    //List of the scripts that should only be active for the local player (ex. PlayerController, MouseLook etc.)
    public MonoBehaviour[] localScripts;

    //List of the GameObjects that should only be active for the local player (ex. Camera, AudioListener etc.)
    public GameObject[] localObjects;

    //Values that will be synced over network
    Rigidbody2D rb;
    Transform playerTransform;

    Vector3 latestPos;
    Quaternion latestRot;
    Vector3 velocity;
    float angularVelocity;

    bool valuesReceived;

    void Start()
    {
        Debug.Assert(player2Sync == null, "PUN2_PlayerSync No player attached!");

        if (photonView.IsMine)
        {
            Debug.Log("PUN2_PlayerSync photon view is mine");
            // Player is local
            rb = player2Sync.GetComponent<Rigidbody2D>();
            playerTransform = player2Sync.transform;

            // Set main healthbar to this player
        }
        else
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
        }
    }

    void Update()
    {
        if (!photonView.IsMine && valuesReceived)
        {
            //Update Object position and Rigidbody parameters
            transform.position = Vector3.Lerp(transform.position, latestPos, Time.deltaTime * 5);
            playerTransform.rotation = Quaternion.Lerp(transform.rotation, latestRot, Time.deltaTime * 5);
            rb.velocity = velocity;
            rb.angularVelocity = angularVelocity;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            //We own this player: send the others our data
            stream.SendNext(playerTransform.position);
            stream.SendNext(playerTransform.rotation);
            stream.SendNext(rb.velocity);
            stream.SendNext(rb.angularVelocity);
        }
        else
        {
            //Network player, receive data
            latestPos = (Vector3)stream.ReceiveNext();
            latestRot = (Quaternion)stream.ReceiveNext();
            velocity = (Vector3)stream.ReceiveNext();
            angularVelocity = (float)stream.ReceiveNext();

            valuesReceived = true;
        }
    }

    //void OnCollisionEnter(Collision contact)
    //{
    //    if (!photonView.IsMine)
    //    {
    //        Transform collisionObjectRoot = contact.transform.root;
    //        if (collisionObjectRoot.tag.Contains("Team"))
    //        {
    //            //Transfer PhotonView of Rigidbody to our local player
    //            photonView.TransferOwnership(PhotonNetwork.LocalPlayer);
    //        }
    //    }
    //}
}


//public class PUN2_PlayerSync : MonoBehaviourPun, IPunObservable
//{
//    //List of the scripts that should only be active for the local player (ex. PlayerController, MouseLook etc.)
//    public MonoBehaviour[] localScripts;

//    //List of the GameObjects that should only be active for the local player (ex. Camera, AudioListener etc.)
//    public GameObject[] localObjects;

//    //Values that will be synced over network
//    Rigidbody2D rb;

//    Vector3 latestPos;
//    Quaternion latestRot;
//    Vector3 velocity;
//    float angularVelocity;

//    bool valuesReceived;

//    void Start()
//    {
//        if (photonView.IsMine)
//        {
//            // Player is local
//            rb = GetComponent<Rigidbody2D>();

//            // Set main healthbar to this player
//        }
//        else
//        {
//            //Player is Remote, deactivate the scripts and object that should only be enabled for the local player
//            for (int i = 0; i < localScripts.Length; i++)
//            {
//                localScripts[i].enabled = false;
//            }
//            for (int i = 0; i < localObjects.Length; i++)
//            {
//                localObjects[i].SetActive(false);
//            }
//        }
//    }

//    void Update()
//    {
//        if (!photonView.IsMine && valuesReceived)
//        {
//            //Update Object position and Rigidbody parameters
//            transform.position = Vector3.Lerp(transform.position, latestPos, Time.deltaTime * 5);
//            transform.rotation = Quaternion.Lerp(transform.rotation, latestRot, Time.deltaTime * 5);
//            rb.velocity = velocity;
//            rb.angularVelocity = angularVelocity;
//        }
//    }

//    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
//    {
//        if (stream.IsWriting)
//        {
//            //We own this player: send the others our data
//            stream.SendNext(transform.position);
//            stream.SendNext(transform.rotation);
//            stream.SendNext(rb.velocity);
//            stream.SendNext(rb.angularVelocity);
//        }
//        else
//        {
//            //Network player, receive data
//            latestPos = (Vector3)stream.ReceiveNext();
//            latestRot = (Quaternion)stream.ReceiveNext();
//            velocity = (Vector3)stream.ReceiveNext();
//            angularVelocity = (float)stream.ReceiveNext();

//            valuesReceived = true;
//        }
//    }
//}
