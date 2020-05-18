using System;
using UnityEngine;

public enum PlanetState
{
    Destroyed = 0,
    Affected,
    Neutral,
    Healed,
    Protected
}

public class PlanetLogic
{
    PlanetState state;
    PlanetState previousState;

    Action<PlanetState,PlanetState> transitionAction;

    public PlanetLogic()
    {
        state = PlanetState.Neutral;
    }

    public void AdvanceState(int winnerTeamID)
    {
        previousState = state;

        if (winnerTeamID == Match.TEAM_1_ID)
        {
            state++;
        }
        else if(winnerTeamID == Match.TEAM_2_ID)
        {
            state--;
        }
        else
        {
            Debug.Log("PlanetLogic AdvanceState() winnerTeamID is not 1 or 2");
        }

        transitionAction.Invoke(previousState, state);

        if (FinalStateReached())
        {
            // End game
            //GameManager.Instance.EndMatch(winnerTeamID);
            Debug.Log("Game has ENDED!");
        }
    }

    bool FinalStateReached()
    {
        if(state == PlanetState.Protected || state == PlanetState.Destroyed)
        {
            return true;
        }

        return false;
    }

    public void SetTransitionAction(Action<PlanetState, PlanetState> action)
    {
        transitionAction = action;
    }
}
