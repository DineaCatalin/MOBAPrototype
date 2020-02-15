using UnityEngine;
using System.Collections;

public class ShieldAbility : Ability
{
    // Cache raycast objects, we will use them to see if we are clicking on a player from our team
    Vector2 mousePosition;
    RaycastHit2D hit;

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

        Debug.Log("CAsting Shield ability");

        // See if the mouse if pointing at the same player
        //ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //if (Physics2D.Raycast(ray, out hit))
        //{
        //    Debug.Log("We clicked on object with tag " + hit.collider.gameObject.tag);

        //    // We clicked on us or our team mate
        //    if (hit.collider.gameObject.tag == teamTag)
        //    {
        //        Player player = hit.collider.gameObject.GetComponent<Player>();
        //        player.ApplyShield(abilityData.stats.hpValue);
        //    }
        //}

        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        hit = Physics2D.Raycast(mousePosition, Vector2.zero, 0);
        if (hit)
        {
            Debug.Log("We clicked on object with tag " + hit.collider.gameObject.tag);

            // We clicked on us or our team mate
            if (hit.collider.gameObject.tag == teamTag)
            {
                Player player = hit.collider.gameObject.GetComponent<Player>();
                player.ApplyShield(abilityData.stats.hpValue);
            }
        }

    }
}
