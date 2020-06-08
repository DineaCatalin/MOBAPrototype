using UnityEngine;

public class MatchFlow : MonoBehaviour
{
    public PlanetCreateTween planetCreateTween;
    public float draftStartAfterPlanetCreation;

    // Start is called before the first frame update
    void Start()
    {
        Flow();
    }

    void Flow()
    {
        planetCreateTween.Execute();
        Invoke("ShowAbilityDraftUI", planetCreateTween.planetActivateTime + draftStartAfterPlanetCreation);
    }

    void ShowAbilityDraftUI()
    {
        EventManager.TriggerEvent(GameEvent.StartRedraft);
    }
}
