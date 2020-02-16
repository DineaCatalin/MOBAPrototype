using UnityEngine;
using System.Collections;

public class RushAbility : Ability
{
    [SerializeField] GameObject rushArea;

    private void Start()
    {
        if (rushArea == null)
        {
            rushArea = GameObject.Find("RushArea" + playerID);
        }
            

        Debug.Log("AAA Gameobject name is " + transform.parent.name);
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
