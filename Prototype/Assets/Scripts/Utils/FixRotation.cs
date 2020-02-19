using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixRotation : MonoBehaviour
{
    Vector3 origin;

    private void Start()
    {
        origin = new Vector3(0, transform.localPosition.y, 0);
    }

    // Update is called once per frame
    void Update()
    {
        transform.localEulerAngles = Vector3.zero;
        transform.rotation = Quaternion.identity;
        transform.localPosition = origin;
    }
}
