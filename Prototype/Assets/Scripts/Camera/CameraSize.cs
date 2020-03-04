using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class CameraSize : MonoBehaviour
{

    // Set this to the in-world distance between the left & right edges of your scene.
    public float sceneWidth = 25;

    Camera _camera;
    void Awake()
    {
        _camera = GetComponent<Camera>();

        SetCameraSize();
    }

    // Adjust the camera's height so the desired scene width fits in view
    void SetCameraSize()
    {
        float unitsPerPixel = sceneWidth / Screen.width;
        float desiredHalfHeight = 0.5f * unitsPerPixel * Screen.height;

        _camera.orthographicSize = desiredHalfHeight;
    }

#if UNITY_EDITOR

    // Adjust the camera's height so the desired scene width fits in view
    // even if the screen/window size changes dynamically.
    void Update()
    {
        SetCameraSize();
    }

#endif
}