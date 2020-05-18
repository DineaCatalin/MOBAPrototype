using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    public Transform track;
    private Transform cachedTransform;
    private Vector3 cachedPosition;

    bool follow;

    void Start()
    {
        cachedTransform = GetComponent<Transform>();
        if (follow)
        {
            cachedPosition = track.position;
            follow = true;
        }
    }

    void Update()
    {
        if (follow && cachedPosition != track.position)
        {
            FollowStep();
        }
    }

    void FollowStep()
    {
        cachedPosition = track.position;
        transform.position = cachedPosition;
    }

    public void StartFollowing()
    {
        Debug.Log("Follow StartFollowing");
        follow = true;
    }

    public void StartFollowWithDelay(float delay)
    {
        Invoke("StartFollowing", delay);
    }

    public void StopFollowing()
    {
        Debug.Log("Follow StopFollowing");
        follow = false;
    }
}
