using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlanetSwitchTween))]
public class PlanetManager : MonoBehaviour
{
    Dictionary<PlanetState, PlanetView> planetMap;
    PlanetLogic planetLogic;

    PlanetView curentPlanet;
    PlanetView previousPlanet;

    PlanetSwitchTween switchTween;

    // Start is called before the first frame update
    void Awake()
    {
        InitPlanetMap();

        planetLogic = new PlanetLogic();
        planetLogic.SetTransitionAction(new System.Action<PlanetState, PlanetState>(AdvanceAction));

        switchTween = GetComponent<PlanetSwitchTween>();

        EventManager.StartListening(GameEvent.PlanetStateAdvance, Advance);
    }

    // TEST
    public void Advance(int winnerTeamID)
    {
        planetLogic.AdvanceState(winnerTeamID);
    }

    void Advance()
    {
        planetLogic.AdvanceState(Match.activeMatch.GetCurrentWinnerTeamID());
    }

    void AdvanceAction(PlanetState previousState, PlanetState currentState)
    {
        // Get planet objects
        previousPlanet = planetMap[previousState];
        curentPlanet = planetMap[currentState];

        switchTween.Execute(previousPlanet, curentPlanet);
    }

    void InitPlanetMap()
    {
        planetMap = new Dictionary<PlanetState, PlanetView>();

        foreach (PlanetView planet in GetComponentsInChildren<PlanetView>())
        {
            planetMap.Add(planet.state, planet);
        }
    }
}
