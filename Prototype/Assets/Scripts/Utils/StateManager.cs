using UnityEngine;
using System.Collections;

// This will just activate/ deactivate the GO that is given 
public class StateManager : MonoBehaviour
{
    [SerializeField] GameObject managedObject;

    public void Activate()
    {
        managedObject.SetActive(true);
    }

    public void Deactivate()
    {
        managedObject.SetActive(false);
    }
}
