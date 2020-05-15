using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LocalParticleSystemManager), typeof(Follow))]
public class Trail : MonoBehaviour
{
    LocalParticleSystemManager trailParticleManager;
    Follow trailFollow;

    [SerializeField] float trailFollowDelay = 0.9f;
    [SerializeField] float trailParticleDelay = 1f;

    private void Awake()
    {
        trailFollow = GetComponent<Follow>();
        trailParticleManager = GetComponent<LocalParticleSystemManager>();
    }

    public void Activate()
    {
        Debug.LogError("Trail Activate " + Time.realtimeSinceStartup);
        Invoke("StartTrailFollow", trailFollowDelay);
        Invoke("StartTrailParticles", trailParticleDelay);
    }

    void StartTrailParticles()
    {
        Debug.LogError("Trail StartTrailParticles " + Time.realtimeSinceStartup);
        trailParticleManager.Play();
    }

    void StartTrailFollow()
    {
        Debug.LogError("Trail StartTrailFollow " + Time.realtimeSinceStartup);
        trailFollow.StartFollowing();
    }

    public void Deactivate()
    {
        Debug.LogError("Trail Deactivate " + Time.realtimeSinceStartup);
        trailFollow.StopFollowing();
        trailParticleManager.Stop();
    }
}
