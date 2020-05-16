using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGraphics : MonoBehaviour
{
    public GameObject grahpics;

    [SerializeField] Trail trail;
    [SerializeField] Shield shield;

    // Update is called once per frame
    public void Enable()
    {
        Debug.Log("PlayerGraphics Enable");

        grahpics.SetActive(true);
        trail.Activate();
    }

    public void EnableAfterBlink()
    {
        Enable();

        if(shield.isActive)
        {
            shield.ActivateShieldGraphics();
        }
    }

    public void Disable()
    {
        Debug.Log("PlayerGraphics Disable");
        grahpics.SetActive(false);
        shield.DeactivateShieldGraphics();
        trail.Deactivate();
    }
}
