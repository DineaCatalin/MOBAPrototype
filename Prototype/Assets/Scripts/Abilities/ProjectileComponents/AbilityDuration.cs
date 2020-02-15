using UnityEngine;
using System.Collections;

// This component will distroy the ability after n seconds
// It is used for Spikes, Roots etc
public class AbilityDuration : MonoBehaviour
{
    [SerializeField] new string name;

    [SerializeField] float duration;

    // Use this for initialization
    void Start()
    {
        duration = AbilityDataCache.GetDataForAbility(name).stats.duration;

        if (duration <= 0f)
            Debug.Log("AbilityDuration Warning duration not set for ability " + name);

        StartCoroutine("Disable");
    }

    IEnumerator Disable()
    {
        yield return new WaitForSeconds(duration);

        gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
