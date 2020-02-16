using UnityEngine;
using System.Collections;

// This will not inherit from AbilityEffect as we don't want to implement ApllyEffect
// We will add a speed value on enter and remove it on exit of the collider
public class RushArea : MonoBehaviour
{
    [SerializeField] new string name;

    AbilityData abilityData;
    float speedBoost;

    private void Start()
    {
        abilityData = AbilityDataCache.GetDataForAbility(name);
        speedBoost = abilityData.stats.dotValue;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == abilityData.description.casterTeamName)
        {
            Player player = collision.GetComponent<Player>();
            player.GetStats().speed += speedBoost;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == abilityData.description.casterTeamName)
        {
            Player player = collision.GetComponent<Player>();
            player.GetStats().speed -= speedBoost;
        }
    }
}
