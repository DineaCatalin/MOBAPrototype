using UnityEngine;
using System.Collections;

public class MimicRotation : MonoBehaviour
{
    [SerializeField] Transform rotateAfter;

    Vector3 rotation;

    private void Start()
    {
        rotateAfter = GetComponentInParent<Player>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        rotation.z = rotateAfter.transform.eulerAngles.z;

        transform.eulerAngles = rotation;
    }
}
