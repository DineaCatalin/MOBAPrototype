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
            Debug.Log("LocalParticleSystemManager particleSystems == null");

        if (deactivateOnAwake)
            ForceStop();
    }

   public void Play()
   {
        foreach (ParticleSystem pSyst in particleSystems)
        {
            pSyst.Play();
        }
    }

   public void Stop()
   {
        foreach (ParticleSystem pSyst in particleSystems)
        {
            pSyst.Stop();
        }
   }

    public void ForceStop()
    {
        foreach (ParticleSystem pSyst in particleSystems)
        {
            pSyst.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
    }
}
