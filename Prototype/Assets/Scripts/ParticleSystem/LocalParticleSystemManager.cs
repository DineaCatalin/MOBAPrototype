using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalParticleSystemManager : MonoBehaviour
{
    ParticleSystem[] particleSystems;

    [SerializeField] bool deactivateOnAwake = true;

    // Start is called before the first frame update
    void Awake()
    {
        particleSystems = GetComponentsInChildren<ParticleSystem>();

        Debug.Log("LocalParticleSystemManager Awake particleSystems " + particleSystems.Length + " for " + name);

        if (particleSystems == null)
            Debug.LogError("LocalParticleSystemManager particleSystems == null");

        if (deactivateOnAwake)
            DeactivateParticleSystems();
    }

   public void ActivateParticleSystems()
   {
        foreach (ParticleSystem pSyst in particleSystems)
        {
            pSyst.Play();
        }
   }

   public void DeactivateParticleSystems()
   {
       foreach (ParticleSystem pSyst in particleSystems)
       {
           pSyst.Stop();
       }
   }
}
