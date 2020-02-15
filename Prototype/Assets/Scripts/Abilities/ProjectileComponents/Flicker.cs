using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flicker : MonoBehaviour
{
    [SerializeField] float duration = 0.2f;

    // Use this for initialization
    void Start()
    {
        StartCoroutine("Disable");
    }

    IEnumerator Disable()
    {
        yield return new WaitForSeconds(duration);

        gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
