using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Will be used to grow a gameobject constantly after a specified time
public class AreaLimiterManager : MonoBehaviour
{
    [SerializeField] Transform[] areaLimiters;

    // We will scale uniformly so there is no reason to cache the whole Vector3
    [SerializeField] float initialScale = 0.1f;
    [SerializeField] float finalScale;

    // When the GO will start growing
    [SerializeField] float startTime;

    [Tooltip("In how many minutes the scale of the object will reach the final scale")]
    [SerializeField] float nrMinTillMaxSize;

    [SerializeField] int callsPerSecond = 10;

    bool finalSizeReached;

    int secondsPerMin = 60;
    float growthIncrementRate;      // How many times a second will the GO grow

    // Start is called before the first frame update
    void Start()
    {
        growthIncrementRate = (finalScale - initialScale) / (nrMinTillMaxSize * secondsPerMin * callsPerSecond);

        for (int i = 0; i < areaLimiters.Length; i++)
        {
            areaLimiters[i].localScale = Vector3.one * initialScale;
        }

        StartCoroutine("ApplyGrowth");
    }

    IEnumerator ApplyGrowth()
    {
        int nrSteps = (int)(nrMinTillMaxSize * secondsPerMin * callsPerSecond);
        Debug.Log("Grow ApplyGrowth nrSteps " + nrSteps);
        int currentStep = 0;

        while (currentStep < nrSteps)
        {
            yield return new WaitForSeconds(1f / callsPerSecond);

            for (int i = 0; i < areaLimiters.Length; i++)
            {
                areaLimiters[i].localScale += Vector3.one * growthIncrementRate;
            }

            currentStep++;
        }
    }
}
