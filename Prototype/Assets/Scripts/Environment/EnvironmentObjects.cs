using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentObjects : MonoBehaviour
{
    public float fadeDuration;
    public LeanTweenType fadeInEaseType;
    public LeanTweenType fadeOutEaseType;
    public EnvironmentPhaseObject[] environmentObjectsPerPhase;

    [HideInInspector]
    public PlanetState currentPlanetState;

    FadeObjectTween[] currentFadeTweens;

    bool showObjects;

    public static EnvironmentObjects Instance;

    private void Awake()
    {
        Instance = this;
        EventManager.StartListening(GameEvent.PlanetStateAdvance, HideObjects);
        EventManager.StartListening(GameEvent.EndRedraft, PrepareShowObjects);
        showObjects = true;
    }

    public void AdvanceOnPlanetChange()
    {
        if (showObjects)
            ShowObjects();
    }

    void PrepareShowObjects()
    {
        showObjects = true;
    }

    void ShowObjects()
    {
        currentFadeTweens = environmentObjectsPerPhase[(int)currentPlanetState].fadeTweens;
        ActivateAndFadeInObjects();
        showObjects = false;
    }

    void HideObjects()
    {
        currentFadeTweens = environmentObjectsPerPhase[(int)currentPlanetState].fadeTweens;
        FadeOutAndDeactivateObjects();
        showObjects = true;
    }

    void FadeOutObjects()
    {
        foreach (var fadeTween in currentFadeTweens)
        {
            fadeTween.FadeOut(fadeDuration, fadeOutEaseType);
        }
    }

    void ActivateAndFadeInObjects()
    {
        foreach (var fadeTween in currentFadeTweens)
        {
            fadeTween.gameObject.SetActive(true);
            fadeTween.FadeIn(fadeDuration, fadeInEaseType);
        }
    }

    void FadeOutAndDeactivateObjects()
    {
        FadeOutObjects();
        Invoke("DeactivateObjects", fadeDuration);
    }

    void DeactivateObjects()
    {
        foreach (var fadeTween in currentFadeTweens)
        {
            fadeTween.gameObject.SetActive(false);
        }
    }

}

[System.Serializable]
public class EnvironmentPhaseObject
{
    public PlanetState planetState;
    public FadeObjectTween[] fadeTweens;
}
