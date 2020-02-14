using UnityEngine;
using System.Collections;

public class ShieldAbility : Ability
{
    // Cache raycast objects, we will use them to see if we are clicking on a player from our team
    Ray ray;
    RaycastHit hit;

    string teamTag;

    public override void Load()
    {
        base.Load();

        teamTag = abilityData.description.casterTeamName;
    }

    private void Start()
    {
        Load();
    }

    public override void Cast()
    {
        base.Cast();

        // See if the mouse if pointing at the same player
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            // We clicked on us or our team mate
            if (hit.collider.gameObject.tag == teamTag)
            {
                Player player = hit.collider.gameObject.GetComponent<Player>();
                player.ApplyShield(abilityData.stats.hpValue, abilityData.stats.duration);
            }

        }
    }

}
