using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetSwitchTween : MonoBehaviour
{
    public Vector3 initialScale;
    public Vector3 zoomScale;

    public LeanTweenType growEaseType;
    public float growTime;

    public LeanTweenType switchEaseType;
    public float switchTime;

    public LeanTweenType shrinkEaseType;
    public float shrinkTime;

    int zOrderVisible = -1;
    int zOrderHidden = 0;

    PlanetView previousPlanet;
    PlanetView currentPlanet;

    public void Execute(PlanetView oldPlanet, PlanetView newPlanet)
    {
        previousPlanet = oldPlanet;
        currentPlanet = newPlanet;

        GrowPlanet();
        Invoke("SwitchPlanets", growTime);
        Invoke("ShrikPlanet", growTime + switchTime);
    }

    void GrowPlanet()
    {
        // Tween previous visible planet to be big
        previousPlanet.Scale(initialScale, zoomScale, growTime, growEaseType);

        // Set previous planet to zOrderHidden
        previousPlanet.transform.position = new Vector3(previousPlanet.transform.position.x, previousPlanet.transform.position.y, zOrderHidden);

        // Set scale for current hidden planet
        currentPlanet.transform.localScale = zoomScale;

        // Set scale for current hidden planet to zOrderHidden
        currentPlanet.transform.position = new Vector3(currentPlanet.transform.position.x, currentPlanet.transform.position.y, zOrderVisible);

    }

    void SwitchPlanets()
    {
        currentPlanet.FadeIn(switchTime, switchEaseType);
        previousPlanet.FadeOut(switchTime, switchEaseType);
    }

    void ShrikPlanet()
    {
        // Set previous planet to small size
        previousPlanet.transform.localScale = initialScale;

        // Tween current planet to small size
        currentPlanet.Scale(zoomScale, initialScale, shrinkTime, shrinkEaseType);
    }
}

    