using UnityEngine;

public class LocalParticleSystemManager : MonoBehaviour
{
    ParticleSystem[] particleSystems;

    [SerializeField] bool deactivateOnAwake = true;

    [HideInInspector] public bool InUse = false;

    float duration;

    // Start is called before the first frame update
    void Awake()
    {
        duration = 0;

        particleSystems = GetComponentsInChildren<ParticleSystem>();

        foreach (var particleSystem in particleSystems)
        {
            if (particleSystem.main.startLifetime.constantMax > duration)
                duration = particleSystem.main.startLifetime.constantMax;
        }

        Debug.Log("LocalParticleSystemManager Awake particleSystems " + particleSystems.Length + " for " + name + " duration " + duration);

        if (particleSystems == null)
            Debug.Log("LocalParticleSystemManager particleSystems == null");

        if (deactivateOnAwake)
            ForceStop();
    }

   public void Play()
   {
        InUse = true;

        foreach (ParticleSystem pSyst in particleSystems)
        {
            pSyst.Play();
        }
   }

   public void PlayAndStop()
   {
        InUse = true;

        foreach (ParticleSystem pSyst in particleSystems)
        {
            pSyst.Play();
        }

        Invoke("Stop", duration);
   }

    public void Stop()
    {
        foreach (ParticleSystem pSyst in particleSystems)
        {
            pSyst.Stop();
        }

        InUse = false;
    }

    public void ForceStop()
    {
        foreach (ParticleSystem pSyst in particleSystems)
        {
            pSyst.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }

        InUse = false; 
    }
}
