using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGraphics : MonoBehaviour
{
    public GameObject grahpics;

    [SerializeField] Trail trail;
    [SerializeField] Shield shield;

    public void SetColor(Color color)
    {
        //spriteRenderer.color = color;
    }

    // Update is called once per frame
    public void Enable()
    {
        Debug.LogError("PlayerGraphics Enable");

        //trailParticleManager.gameObject.SetActive(true);
        grahpics.SetActive(true);
        //trailFollow.CatchUp();
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
        Debug.LogError("PlayerGraphics Disable");
        //trailParticleManager.gameObject.SetActive(false);
        grahpics.SetActive(false);
        shield.DeactivateShieldGraphics();
        trail.Deactivate();
    }
}
