using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCursor : MonoBehaviour
{
    Vector3 position;

    // Update is called once per frame
    void Update()
    {
        position = Input.mousePosition;
        position = Utils.Instance.CameraScreenToWorldPoint(position);
        position.z = 0;
        transform.position = position;
    }
}
