using UnityEngine;
using System.Collections;

public class LookAtMouse : MonoBehaviour
{
    Vector3 direction;
    float angle;
    [SerializeField] float angleCorrection = 90;

    // Update is called once per frame
    void Update()
    {
        direction = Input.mousePosition - Utils.CameraScreenToWorldPoint(transform.position);
        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - angleCorrection;  // Angle correction as the sprites rotates 90 degrees extra
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
