using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetSwitchTween : MonoBehaviour
{
    public Vector3 initialScale;
    public Vector3 zoomScale;

    public LeanTweenType growEaseType;
    public float growTime;
    public float switchDelayAfterGrow;

    public LeanTweenType switchEaseType;
    public float switchTime;
    public float shrinkDelayAfterSwitch;

    public LeanTweenType shrinkEaseType;
    public float shrinkTime;

    PlanetView previousPlanet;
    PlanetView currentPlanet;

    float shrinkTweenTime;
    float switchTweenTime;

    public float finalTransitionDelay = 0.2f;

    private void Awake()
    {
        switchTweenTime = growTime + switchDelayAfterGrow;
        shrinkTweenTime = growTime + switchTime + shrinkDelayAfterSwitch;
    }

    public void Execute(PlanetView oldPlanet, PlanetView newPlanet)
    {
        previousPlanet = oldPlanet;
        currentPlanet = newPlanet;

        GrowPlanet();

        if(currentPlanet.IsFinalState())
        {
            HandleFinalTransition();
        }
        else
        {
            HandleTransition();
        }
        
    }

    void GrowPlanet()
    {
        // Tween previous visible planet to be big
        previousPlanet.Scale(initialScale, zoomScale, growTime, growEaseType);

        // Set scale for current hidden planet
        currentPlanet.transform.localScale = zoomScale;
    }

    void SwitchPlanets()
    {
        PlayParticles();

        currentPlanet.FadeIn(switchTime, switchEaseType);
        previousPlanet.FadeOut(switchTime, switchEaseType);
    }

    void InstantSwitchPlanets()
    {
        currentPlanet.FadeIn(0f, switchEaseType);
        previousPlanet.FadeOut(0f, switchEaseType);
    }

    void ShrikPlanet()
    {
        StopParticles();

        // Set previous planet to small size
        previousPlanet.transform.localScale = initialScale;

        // Tween current planet to small size
        currentPlanet.Scale(zoomScale, initialScale, shrinkTime, shrinkEaseType);
    }

    void HandleTransition()
    {
        Invoke("SwitchPlanets", switchTweenTime);
        Invoke("ShrikPlanet", shrinkTweenTime);
    }

    void HandleFinalTransition()
    {
        Invoke("PlayParticles", switchTime);
        Invoke("InstantSwitchPlanets", switchTweenTime + finalTransitionDelay);
    }

    void PlayParticles()
    {
        currentPlanet.PlayParticles();
    }

    void StopParticles()
    {
        currentPlanet.StopParticles();
    }
}

    