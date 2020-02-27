using UnityEngine;
using System.Collections;

public class RushAbility : Ability
{
    StateManager rushAreaManager;

    private void Start()
    {
        // Find and cache the GO of the RushArea
        rushAreaManager = GameObject.Find("RushAreaContainer" + playerID).GetComponent<StateManager>();

        // Deactivateso that the player doesn't start wthe
        // Rush ability activated
        rushAreaManager.Deactivate();
    }   

    public override void Cast()
    {
        base.Cast();

        rushAreaManager.Activate();

        StartCoroutine(RemoveRushArea(abilityData.stats.duration));
    }

    IEnumerator RemoveRushArea(float duration)
    {
        yield return new WaitForSeconds(duration);

        rushAreaManager.Deactivate();
    }
}
