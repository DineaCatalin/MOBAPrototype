using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonLocalPlayerMovement : MonoBehaviour
{
    Transform playerTransform;
    bool locked;

    void Awake()
    {
        playerTransform = transform;
        locked = false;
    }

    public void Move(Vector3 moveTo)
    {
        if(!locked)
            playerTransform.position = moveTo;
    }

    public void Lock()
    {
        locked = true;
    }

    public void Unlock()
    {
        locked = false;
    }
}
