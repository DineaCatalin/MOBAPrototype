using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTeleportation : MonoBehaviour
{
    Rigidbody2D playerRigidbody;

    float movePlayerDelay = 0.035f; 
    float activatePlayerdelay = 0.15f;

    [SerializeField] NonLocalPlayerMovement nonLocalPlayerMovement;

    Player player;

    Vector2 teleportLocation;

    // Start is called before the first frame update
    void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        player = GetComponent<Player>();
        nonLocalPlayerMovement = GetComponent<NonLocalPlayerMovement>();
    }

    public void Teleport(Vector2 location)
    {
        //nonLocalPlayerMovement.Lock();
        Debug.Log("PlayerTeleportation Teleport player.Deactivate();" + Time.realtimeSinceStartup);
        nonLocalPlayerMovement.Lock();
        player.DeactivateForBlink();
        teleportLocation = location;
        Debug.Log("PlayerTeleportation Teleport teleportLocation = location;" + Time.realtimeSinceStartup);

        //MovePlayer();
        Invoke("MovePlayer", movePlayerDelay);
        Invoke("ActivatePlayer", activatePlayerdelay);
    }

    void MovePlayer()
    {
        playerRigidbody.isKinematic = true;
        transform.position = teleportLocation;
        playerRigidbody.isKinematic = false;
        Debug.Log("PlayerTeleportation MovePlayer transform.position = teleportLocation; " + Time.realtimeSinceStartup);
    }

    void ActivatePlayer()
    {
        Debug.Log("PlayerTeleportation ActivatePlayer player.ActivateGraphics(); " + Time.realtimeSinceStartup);
        player.ActivateGraphics();
        nonLocalPlayerMovement.Unlock();
    }
}
