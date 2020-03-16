using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flicker : MonoBehaviour
{
    [SerializeField] float duration = 0.2f;

    // We use OnEnable because the GO that uses this component is used in an ObjectPool
    void OnEnable()
    {
        StartCoroutine("Disable");
    }

    IEnumerator Disable()
    {
        yield return new WaitForSeconds(duration);
        Debug.Log("Flicker Disable");
        gameObject.SetActive(false);
        //Destroy(gameObject);
    }
}
