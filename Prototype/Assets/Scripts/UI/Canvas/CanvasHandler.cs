using System;
using UnityEngine;

public class CanvasHandler : MonoBehaviour
{
    [SerializeField] bool alsoModifyGameObjectActiveState;

    [SerializeField] GameEvent[] eventForActivate;
    [SerializeField] GameEvent[] eventsForDeactivate;

    [SerializeField] Tween[] onActivateTweens;
    [SerializeField] Tween[] onDeactivateTweens;

    Canvas canvas;

    Action<bool> setStateAction;

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

        if(alsoModifyGameObjectActiveState)
        {
            Debug.Log("CanvasHandler Awake useGameObjectForDeactivation " + name);
            setStateAction = new Action<bool>(SetCanvasAndGOState);
        }
        else
        {
            Debug.Log("CanvasHandler Awake !useGameObjectForDeactivation " + name);
            setStateAction = new Action<bool>(SetCanvasState);
        }
    }

    public void Open()
    {
        Debug.Log("CanvasHandler Open " + name);
        setStateAction.Invoke(true);

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

        setStateAction.Invoke(false);
        Debug.Log("CanvasHandler Close " + name);
    }

    void SetCanvasState(bool active)
    {
        canvas.enabled = active;
    }

    void SetCanvasAndGOState(bool active)
    {
        gameObject.SetActive(active);
        canvas.enabled = active;
    }
}
