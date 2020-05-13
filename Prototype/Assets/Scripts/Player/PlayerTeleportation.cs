using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTeleportation : MonoBehaviour
{
    Rigidbody2D playerRigidbody;

    Vector2 velocity;
    float angularVelocity;

    // Start is called before the first frame update
    void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
    }

    public void Teleport(Vector2 location)
    {
        //velocity = playerRigidbody.velocity;
        //angularVelocity = playerRigidbody.angularVelocity;

        //playerRigidbody.isKinematic = true;

        transform.position = location;

        //playerRigidbody.isKinematic = false;

        //playerRigidbody.velocity = velocity;
        //playerRigidbody.angularVelocity = angularVelocity;
    }
}
