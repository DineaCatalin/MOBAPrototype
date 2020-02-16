using UnityEngine;
using System.Collections;

public class RushAbility : Ability
{
    [SerializeField] GameObject rushArea;

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
