using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetCreateTween : MonoBehaviour
{
    [SerializeField] GameObject vortex;
    [SerializeField] GameObject explosion;
    [SerializeField] FadeObjectTween planetFadeTween;

    [SerializeField] LeanTweenType vortexShrinkTweenType;
    [SerializeField] Vector3 vortexInitialScale;
    [SerializeField] Vector3 vortexFinalScale;
    [SerializeField] float vortexShrinkTime;

    [SerializeField] float explosionActivateTime;

    public float planetActivateTime;
    public float planetFadeInTime;

    //LocalParticleSystemManager vortexParticles;
    //LocalParticleSystemManager explosionParticles;

    // Start is called before the first frame update
    void Awake()
    {
        //vortexParticles = vortex.GetComponent<LocalParticleSystemManager>();
        //explosionParticles = explosion.GetComponent<LocalParticleSystemManager>();

        explosion.SetActive(false);
        planetFadeTween.FadeOut(0);
        vortex.transform.localScale = vortexInitialScale;
    }

    public void Execute()
    {
        LeanTween.scale(vortex, vortexFinalScale, vortexShrinkTime).setEase(vortexShrinkTweenType);
        Invoke("ActivateExplosion", explosionActivateTime);
        Invoke("DeactivateVortex", vortexShrinkTime);
        Invoke("ActivatePlanet", planetActivateTime);
    }

    void ActivateExplosion()
    {
        explosion.SetActive(true);
    }

    void DeactivateVortex()
    {
        vortex.SetActive(false);
    }

    void ActivatePlanet()
    {
        planetFadeTween.FadeIn(planetFadeInTime);
    }
}
