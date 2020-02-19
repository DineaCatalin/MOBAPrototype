using UnityEngine;
using System.Collections;

public class RushAbility : Ability
{
    [SerializeField] GameObject rushArea;

    private void Start()
    {
        if (rushArea == null)
        {
            // Due to the fact that the player is instantiated over the network the object
            // we are working on now is a clone so it has the string "(Clone)" attached to its name
            rushArea = GameObject.Find("RushArea" + playerID);
        }
    }

    public override void Cast()
    {
        base.Cast();

        rushArea.SetActive(true);

        StartCoroutine(RemoveRushArea(abilityData.stats.duration));
    }

    IEnumerator RemoveRushArea(float duration)
    {
        yield return new WaitForSeconds(duration);

        rushArea.SetActive(false);
    }
}
