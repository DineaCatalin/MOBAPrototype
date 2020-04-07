﻿using UnityEngine;
using System.Collections;

// This component will distroy the ability after n seconds
// It is used for Spikes, Roots etc
public class AbilityDuration : MonoBehaviour
{
    [SerializeField] new string name;

    float duration;

    private void Start()
    {
        EventManager.StartListening("StartRound", new System.Action(Deactivate));
        EventManager.StartListening("StartRedraft", new System.Action(Deactivate));
    }

    // We use OnEnable because the GO that uses this component is used in an ObjectPool
    void OnEnable()
    {
        duration = AbilityDataCache.GetDataForAbility(name).stats.duration;

        if (duration <= 0f)
            Debug.Log("AbilityDuration Warning duration not set for ability " + name);

        StartCoroutine("Disable");
    }

    IEnumerator Disable()
    {
        yield return new WaitForSeconds(duration);
        Debug.Log("AbilityDuration Disable");
        gameObject.SetActive(false);
    }

    void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
