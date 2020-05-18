using System;
using System.Collections.Generic;
using UnityEngine;

public class CanvasHandler : MonoBehaviour
{
    [SerializeField] bool alsoModifyGameObjectActiveState;

    [SerializeField] GameEvent[] eventForActivate;
    [SerializeField] GameEvent[] eventsForDeactivate;

    Tween[] onActivateTweens;
    Tween[] onDeactivateTweens;

    Canvas canvas;

    Action<bool> setStateAction;

    private void Awake()
    {
        canvas = GetComponent<Canvas>();

        Action openAction = new Action(Open);
        Action closeAction = new Action(Close);

        SetTweens();

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

    //private void Start()
    //{
    //    SetTweens();
    //}

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

    void SetTweens()
    {
        Tween[] tweens = GetComponentsInChildren<Tween>();
        Debug.Log("CanvasHandler SetTweens tweens " + tweens.Length);

        List<Tween> onActiveTweenList = new List<Tween>();
        List<Tween> onDeactiveTweenList = new List<Tween>();

        foreach (var tween in tweens)
        {
            if(tween.useOnActivate)
            {
                Debug.Log("CanvasHandler SetTweens tween.useOnActivate " + tween.name);
                onActiveTweenList.Add(tween);
            }
            else
            {
                Debug.Log("CanvasHandler SetTweens !tween.useOnActivate ");
                onDeactiveTweenList.Add(tween);
            }
        }

        Debug.Log("CanvasHandler SetTweens onActiveTweenList " + onActiveTweenList.Count);

        onActivateTweens = new Tween[onActiveTweenList.Count];
        onDeactivateTweens = new Tween[onDeactiveTweenList.Count];

        for (int i = 0; i < onActiveTweenList.Count; i++)
        {
            onActivateTweens[i] = onActiveTweenList[i];
        }

        for (int i = 0; i < onDeactiveTweenList.Count; i++)
        {
            onDeactivateTweens[i] = onDeactiveTweenList[i];
        }

        Debug.Log("CanvasHandler " + name + " SetTweens onActivateTweens " + onActivateTweens.Length);
    }
}
