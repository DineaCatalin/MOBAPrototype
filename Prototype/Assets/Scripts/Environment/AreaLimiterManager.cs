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

    // We use this so we can cancel the grow coroutine and restart it when we start a new round
    Coroutine growCoroutine;

    public void StartZoneExpansion()
    {
        // GameManager will start this on match start
        StartCoroutine("ApplyGrowth");
    }

    // Start is called before the first frame update
    void Start()
    {
        growthIncrementRate = (finalScale - initialScale) / (nrMinTillMaxSize * secondsPerMin * callsPerSecond);

        for (int i = 0; i < areaLimiters.Length; i++)
        {
            areaLimiters[i].localScale = Vector3.one * initialScale;
            areaLimiters[i].gameObject.SetActive(false);
        }

        EventManager.StartListening("StartMatch", new System.Action(OnMatchStart));
        EventManager.StartListening("StartRound", new System.Action(OnRoundStart));

        Invoke("PositionAreaLimiters", 1f);
    }

    // This will position the area containers ( 2 in this case )
    // Will need to rewrite this function to make it position more then 2 elements
    void PositionAreaLimiters()
    {
        Vector3 environmentSize = EnvironmentManager.Instance.environmentSize;

        // Set 1st area limiter in the upper left corner
        areaLimiters[0].transform.position = new Vector3(-environmentSize.x, environmentSize.y, 0);
        areaLimiters[0].gameObject.SetActive(true);

        // Set 2nd area limiter in the lower right corner
        areaLimiters[1].transform.position = new Vector3(environmentSize.x, -environmentSize.y, 0);
        areaLimiters[1].gameObject.SetActive(true);

        Debug.Log("SYNC_ENV_POS Area Limiter NW " + areaLimiters[0].transform.position + " Area Limiter SE " + areaLimiters[1].transform.position);
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

    void OnMatchStart()
    {
        growCoroutine = StartCoroutine(ApplyGrowth());
    }

    void OnRoundStart()
    {
        StopCoroutine(growCoroutine);

        for (int i = 0; i < areaLimiters.Length; i++)
        {
            areaLimiters[i].localScale = Vector3.one * initialScale;
        }

        growCoroutine = StartCoroutine(ApplyGrowth());
    }
}
