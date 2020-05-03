using UnityEngine;
using System.Collections;

// This component will distroy the ability after n seconds
// It is used for Spikes, Roots etc
public class AbilityDuration : AbilityComponent
{
    [SerializeField] new string name;

    ProjectileVisuals visuals;

    float duration;

    private void Awake()
    {
        duration = AbilityDataCache.GetDataForAbility(name).stats.duration;
        Debug.Log("AbilityDuration Duration for " + name + " is " + duration);


        if (duration <= 0f)
            Debug.Log("AbilityDuration Warning duration not set for ability " + name);

        AbilitySpawnTween tween = GetComponent<AbilitySpawnTween>();
        if (tween)
            duration += tween.duration;

        visuals = GetComponent<ProjectileVisuals>();
    }

    private void Start()
    {
        EventManager.StartListening(GameEvent.StartRound, new System.Action(Deactivate));
        EventManager.StartListening(GameEvent.StartRedraft, new System.Action(Deactivate));
    }

    // We use OnEnable because the GO that uses this component is used in an ObjectPool
    void OnEnable()
    {
        Invoke("Deactivate", duration);
    }

    void Deactivate()
    {
        visuals.Deactivate();
    }
}
