using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    // TODO: Load this from a config later
    // PLS don't leave it like this
    public static float respawnCooldown = 5f;

    public static GameManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void KillAndRespawnPlayer(float respawnTimer, Player player)
    {
        StartCoroutine(SpawnPlayerWithDelay(respawnTimer, player));
    }

    IEnumerator SpawnPlayerWithDelay(float respawnTimer, Player player)
    {
        // Wait respawntimer out
        yield return new WaitForSeconds(respawnTimer);

        // "Respawn" player by activating the player's GO and reset health and mana
        // The player will take care of doing this
        player.Reset();
    }
}
