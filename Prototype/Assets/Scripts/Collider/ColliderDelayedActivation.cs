using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderDelayedActivation : MonoBehaviour
{
    public float activationTime; 

    Collider2D objectCollider;

    // Start is called before the first frame update
    void Awake()
    {
        objectCollider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Start()
    {
        Invoke("EnableCollider", activationTime);
    }

    void EnableCollider()
    {
        objectCollider.enabled = true;
    }

    private void OnDisable()
    {
        objectCollider.enabled = false;
    }
}
