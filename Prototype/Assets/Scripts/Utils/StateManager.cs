using UnityEngine;
using System.Collections;

// This will just activate/ deactivate the GO that is given 
public class StateManager : MonoBehaviour
{
    [SerializeField] GameObject managedObject;

    Coroutine deactivate;

    public void Activate(float duration)
    {
        Debug.Log("StateManager Activate " + managedObject.name);
        managedObject.SetActive(true);

        if (deactivate != null)
            StopCoroutine(deactivate);

        deactivate = StartCoroutine(Deactivate(duration));
    }

    IEnumerator Deactivate(float duration)
    {
        yield return new WaitForSeconds(duration);

        Debug.Log("StateManager Activate " + managedObject.name);

        managedObject.SetActive(false);
    }

    public void SetLayer(int layer)
    {
        managedObject.layer = layer;
    }

    public void Deactivate()
    {
        managedObject.SetActive(false);
    }
}
