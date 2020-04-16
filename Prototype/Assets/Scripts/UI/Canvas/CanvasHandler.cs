using System;
using UnityEngine;

public class CanvasHandler : MonoBehaviour
{
    public GameEvent[] eventForActivate;
    public GameEvent[] eventsForDeactivate;

    public Tween[] onActivateTweens;
    public Tween[] onDeactivateTweens;

    Canvas canvas;

    private void Awake()
    {
        canvas = GetComponent<Canvas>();

        Action openAction = new Action(Open);
        Action closeAction = new Action(Close);

        foreach (GameEvent gameEvent in eventForActivate)
        {
            EventManager.StartListening(gameEvent, openAction);
        }

        foreach (GameEvent gameEvent in eventsForDeactivate)
        {
            EventManager.StartListening(gameEvent, closeAction);
        }
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
