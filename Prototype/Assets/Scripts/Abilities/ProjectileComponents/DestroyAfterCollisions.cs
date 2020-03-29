﻿using UnityEngine;
using System.Collections;

// The wall will be destroyed if hit a number of times by abilities
public class DestroyAfterCollisions : MonoBehaviour
{
    [SerializeField] int numCollisions;

    private void Start()
    {
        EventManager.StartListening("StartRound", new System.Action(Destroy));
        EventManager.StartListening("StartRedraft", new System.Action(Destroy));
    }

    public void ApplyDamage()
    {
        numCollisions--;

        if (numCollisions <= 0)
            this.Destroy();
    }

    public void Destroy()
    {
        //Destroy(gameObject);
        gameObject.SetActive(false);
    }
}
