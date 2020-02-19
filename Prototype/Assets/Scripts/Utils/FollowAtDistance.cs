using UnityEngine;
using System.Collections;

public class FollowAtDistance : MonoBehaviour
{
    Vector3 offset;

    [SerializeField] Transform followTarget;

    // Use this for initialization
    void Start()
    {
        offset = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = followTarget.position + offset;
    }
}
