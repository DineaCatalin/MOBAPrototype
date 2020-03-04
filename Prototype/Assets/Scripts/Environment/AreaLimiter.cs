using UnityEngine;
using System.Collections;

public class AreaLimiter : MonoBehaviour
{
    [SerializeField] int damagePerTick;
    [SerializeField] int damageInterval;

    private void Start()
    {
        // TODO: Load values from config
        //damagePerTick = 10;
        //damageInterval = 1;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("AreaLimiter OnTriggerEnter2D");

        // The area limiter has hit a player
        if (other.tag.Contains("Team"))
        {
            //GameManager.Instance.ApplyArenaLimiterDamage(damagePerTick, damageInterval, playerID);

            Player player = other.GetComponent<Player>();
            Debug.Log("AreaLimiter OnTriggerEnter2D applying damage to player " + player.GetID());
            player.ApplyArenaLimiterDOT(damagePerTick, damageInterval);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("AreaLimiter OnTriggerExit2D");

        // The area limiter has hit a player
        if (other.tag.Contains("Team"))
        {
            //GameManager.Instance.DisableArenaLimiterDOTRPC(playerID);

            Player player = other.GetComponent<Player>();
            Debug.Log("AreaLimiter OnTriggerExit2D stopping damage to player " + player.GetID());
            player.DisableArenaLimiterDOT();
        }
    }
}
