using UnityEngine;
using System.Collections;

public class StopCanvasRotation : MonoBehaviour
{
    RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void LateUpdate()
    {
        Debug.Log("StopCanvasRotation rotation before " + rectTransform.rotation);
        rectTransform.Rotate( Vector3.zero );
        Debug.Log("StopCanvasRotation rotation after " + rectTransform.rotation);
    }
}
