using UnityEngine;
using System.Collections;

// This component will distroy the ability after n seconds
// It is used for Spikes, Roots etc
public class AbilityDuration : MonoBehaviour
{
    public float duration;

    // Use this for initialization
    void Start()
    {
        StartCoroutine("Disable");
    }

    IEnumerator Disable()
    {
        yield return new WaitForSeconds(duration);

        gameObject.SetActive(false);
    }
}
