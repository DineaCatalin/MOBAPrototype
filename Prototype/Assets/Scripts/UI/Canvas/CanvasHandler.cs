using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasHandler : MonoBehaviour
{
    public Tween[] onActivateTweens;
    public Tween[] onDeactivateTweens;

    Canvas canvas;

    private void Awake()
    {
        canvas = GetComponent<Canvas>();
    }

    public void Open()
    {
        Debug.Log("CanvasHandler Open " + name);
        canvas.enabled = true;

        foreach (Tween tween in onActivateTweens)
        {
            tween.Execute();
        }
    }

    public void Close()
    {
        foreach (Tween tween in onDeactivateTweens)
        {
            tween.Execute();
        }

        canvas.enabled = false;
        Debug.Log("CanvasHandler Close " + name);
    }
}
