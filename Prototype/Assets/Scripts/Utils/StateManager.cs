using UnityEngine;
using System.Collections;

// This will just activate/ deactivate the GO that is given 
public class StateManager : MonoBehaviour
{
    [SerializeField] GameObject managedObject;

    public void Activate(float duration)
    {
        managedObject.SetActive(true);

        StartCoroutine(Deactivate(duration));
    }

    IEnumerator Deactivate(float duration)
    {
        yield return new WaitForSeconds(duration);

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
