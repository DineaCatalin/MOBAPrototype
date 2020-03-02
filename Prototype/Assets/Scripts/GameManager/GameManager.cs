using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class GameManager : MonoBehaviour
{
    // TODO: Load this from a config later
    // PLS don't leave it like this
    public static float RESPAWN_COOLDOWN = 5f;
    public static float MAX_PLAYERS = 4;

    const float ADD_PLAYER_DELAY = 1f;

    public static GameManager Instance;

    PhotonView photonView;

    Dictionary<int, Player> playerMap;

    // Holds team and score
    Match match;

    private void Awake()
    {
        Instance = this;
        photonView = GetComponent<PhotonView>();
        playerMap = new Dictionary<int, Player>();

        match = new Match();
    }

    public void KillAndRespawnPlayer(float respawnTimer, int playerID, int teamID)
    {
        if(photonView.IsMine)
        {
            // Increase score here so that only 1 client will do it
            match.IncreaseTeamScore(teamID);

            // Call RPC so that others update their score
            photonView.RPC("UpdateScoreRPC", RpcTarget.OthersBuffered, teamID);

        }
        photonView.RPC("KillAndRespawnPlayerRPC", RpcTarget.All, respawnTimer, playerID, teamID);
    }

    [PunRPC]
    void UpdateScoreRPC(int teamIDKilledPlayer)
    {
        match.IncreaseTeamScore(teamIDKilledPlayer);
    }

    [PunRPC]
    public void KillAndRespawnPlayerRPC(float respawnTimer, int playerID, int teamIDKilledPlayer)
    {
        if(playerMap[playerID] != null)
        {
            playerMap[playerID].Deactivate();
            StartCoroutine(SpawnPlayerWithDelay(respawnTimer, playerID));

            // Add the score to the team that has killed the player with @playerID
            Debug.Log("GameManager KillAndRespawnPlayerRPC score. Player has been killed " + playerID);
        }
    }

    IEnumerator SpawnPlayerWithDelay(float respawnTimer, int playerID)
    {
        // Wait respawntimer out
        yield return new WaitForSeconds(respawnTimer);

        // "Respawn" player by activating the player's GO and reset health and mana
        // The player will take care of doing this
        playerMap[playerID].Reset();
    }

    public void AddPlayer(int playerID)
    {
        photonView.RPC("AddPlayerRPC", RpcTarget.All, playerID);
    }

    [PunRPC]
    public void AddPlayerRPC(int playerID)
    {
        Debug.Log("GameManager AddPlayerRPC " + playerID);

        // Find player with playerID. We would have sent the player directly as a parameter to the RPC
        // but it doens't work, so we send the ID and every client has to find the Player by itself
        // We say it's fine because this is triggered only at the beginning of the match when the players are added
        GameObject player = GameObject.Find("Player" + playerID);

        if(player == null)
        {
            StartCoroutine(AddPlayerDelayed(playerID, ADD_PLAYER_DELAY));
        }
        else
        {
            AddPlayerNoDelay(playerID);
        }
    }

    // We will call this coroutine so that the game has time to instantiate the player
    // so that we can find it 
    IEnumerator AddPlayerDelayed(int playerID, float delay)
    {
        yield return new WaitForSeconds(delay);

        Debug.Log("GameManager AddPlayerDelayed " + playerID);

        AddPlayerNoDelay(playerID);
    }

    void AddPlayerNoDelay(int playerID)
    {
        Player player = GameObject.Find("Player" + playerID).GetComponent<Player>();

        int teamID = GetTeamToAssign();

        player.SetTeamSpecificData(teamID);
        //match.AssignPlayer(player, teamID);

        if (!playerMap.ContainsKey(playerID))
        {
            playerMap.Add(playerID, player);

            // Make sure the other clients have all the players added to their local map
            SyncPlayerMap();
        }
    }

    public Player GetPlayer(int playerID)
    {
        return playerMap[playerID];
    }

    public void ActivatePlayer(int playerID)
    {
        photonView.RPC("ActivatePlayerRPC", RpcTarget.All, playerID);
    }

    [PunRPC]
    public void ActivatePlayerRPC(int playerID)
    {
        Debug.Log("ActivatePlayerRPC getting player " + playerID + " from map");
        playerMap[playerID].Activate();
    }

    public void DeactivatePlayer(int playerID)
    {
        photonView.RPC("DeactivatePlayerRPC", RpcTarget.All, playerID);
    }

    [PunRPC]
    public void DeactivatePlayerRPC(int playerID)
    {
        playerMap[playerID].Deactivate();
    }

    int GetTeamToAssign()
    {
        int teamID = (int)PhotonNetwork.CurrentRoom.CustomProperties["spawnedPlayerTeamID"];

        if(teamID == 0)
            teamID = 1;
        else
            teamID = (teamID % 2) + 1;

        // Assign the new value to the room properties
        Hashtable roomProperties = new Hashtable();
        roomProperties.Add("spawnedPlayerTeamID", teamID);
        PhotonNetwork.CurrentRoom.SetCustomProperties(roomProperties);

        Debug.Log("GameManager GetTeamToAssign teamID is " + teamID);
        
        return teamID;
    }

    // The controller will take care of passing the message to the player to activate it's shield
    // from the ShieldAbility. It will do this because the photonView is attached to the GO the controller is attached,
    // so we can't call RPC's from the components on the child object aka the Player component
    public void ActivatePlayerShield(int armor, int playerID)
    {
        Debug.Log("GameManager ActivatePlayerShield with " + armor + " armor for player " + playerID);
        photonView.RPC("ActivatePlayerShieldRPC", RpcTarget.All, armor, playerID);
    }

    [PunRPC]
    void ActivatePlayerShieldRPC(int armor, int playerID)
    {
        Debug.Log("GameManager ActivatePlayerShieldRPC with " + armor + " armor for player " + playerID);
        if (playerMap[playerID] == null)
            return;

        playerMap[playerID].ActivateShield(armor);
    }

    public void KnockOutPlayer(int force, int damage, int playerID)
    {
        Debug.Log("GameManager KnockOutPlayer");
        playerMap[playerID].Knockout(force, damage);
    }

    public void RemovePlayer(int playerID)
    {
        photonView.RPC("RemovePlayerRPC", RpcTarget.All, playerID);
    }

    [PunRPC]
    void RemovePlayerRPC(int playerID)
    {
        if(playerMap.ContainsKey(playerID))
        {
            Debug.Log("Removing from playerMap player : " + playerID);
            playerMap.Remove(playerID);
        }
    }

    void SyncPlayerMap()
    {
        int[] playerIDs = new int[playerMap.Count];
        int index = 0;

        // Get all the players we have in our map locally
        foreach (KeyValuePair<int, Player> mapValue in playerMap)
        {
            playerIDs[index] = mapValue.Key;
            index++;
        }

        photonView.RPC("SyncPlayerMapRPC", RpcTarget.Others, playerIDs);
    }

    [PunRPC]
    void SyncPlayerMapRPC(int[] playerIDs)
    {
        int playerID;

        // Loop all the ids in the map to see if our local map has all of them
        for (int i = 0; i < playerIDs.Length; i++)
        {
            playerID = playerIDs[i];

            // We found an ID that is not in our local map
            if (!playerMap.ContainsKey(playerID))
            {
                Debug.Log("GameManager SyncPlayerMapRPC Finding player with ID " + playerID);
                GameObject playerGO = GameObject.Find("Player" + playerID);

                if(playerGO != null)
                {
                    Player player = playerGO.GetComponent<Player>();
                    playerMap.Add(playerID, player);
                }
            }
        }
    }

    public void ActivateRushArea(float duration, int playerID)
    {
        photonView.RPC("ActivateRushAreaRPC", RpcTarget.All, duration, playerID);
    }

    [PunRPC]
    void ActivateRushAreaRPC(float duration, int playerID)
    {
        if(playerMap.ContainsKey(playerID))
        {
            playerMap[playerID].ActivateRushArea(duration);
        }
    }

    public void PullPlayer(Vector3 pullTarget, int force, int damage, int playerID)
    {
        photonView.RPC("PullPlayerRPC", RpcTarget.All, pullTarget, force, damage, playerID);
    }

    [PunRPC]
    void PullPlayerRPC(Vector3 pullTarget, int force, int damage, int playerID)
    {
        if(playerMap[playerID] != null)
        {
            playerMap[playerID].PullToLocation(pullTarget, force, damage);
        }
    }

    public void HealPlayerNoStacks(int initialHeal, int healTicks, int healTickValue, int playerID)
    {
        photonView.RPC("HealPlayerNoStacksRPC", RpcTarget.All, initialHeal, healTicks, healTickValue, playerID);
    }

    [PunRPC]
    void HealPlayerNoStacksRPC(int initialHeal, int healTicks, int healTickValue, int playerID)
    {
        if(playerMap[playerID] != null)
        {
            playerMap[playerID].WaterRainHeal(initialHeal, healTicks, healTickValue);
        }
    }

    public void PlayerItemPickup(ItemData itemData ,int playerID)
    {
        switch(itemData.name)
        {
            case "HP Sphere":
            {
                photonView.RPC("PlayerHealthPickupRPC", RpcTarget.All, itemData.health, playerID);
                break;
            }
            case "Mana Sphere":
            {
                    photonView.RPC("PlayerManaPickupRPC", RpcTarget.All, itemData.mana, playerID);
                    break;
            }
            case "Power Sphere":
            {
                    photonView.RPC("PlayerPowerPickupRPC", RpcTarget.All, itemData.duration, itemData.powerMultiplier, playerID);
                    break;
            }
            case "Speed Sphere":
            {
                    photonView.RPC("PlayerSpeedPickupRPC", RpcTarget.All, itemData.duration, itemData.speedMultiplier, playerID);
                    break;
            }
            default:
                return;
        }
    }

    [PunRPC]
    void PlayerHealthPickupRPC(int heath, int playerID)
    {
        if (playerMap[playerID] != null)
        {
            playerMap[playerID].PickUpHPItem(heath);
        }
    }

    [PunRPC]
    void PlayerManaPickupRPC(int mana, int playerID)
    {
        if (playerMap[playerID] != null)
        {
            playerMap[playerID].PickUpManaItem(mana);
        }
    }

    [PunRPC]
    void PlayerPowerPickupRPC(float duration, float power, int playerID)
    {
        if (playerMap[playerID] != null)
        {
            playerMap[playerID].PickUpPowerItem(duration, power);
        }
    }

    [PunRPC]
    void PlayerSpeedPickupRPC(float duration, float speed, int playerID)
    {
        if (playerMap[playerID] != null)
        {
            playerMap[playerID].PickUpSpeedItem(duration, speed);
        }
    }

    public void SlowPlayer(int duration, int slowValue, int playerID)
    {
        photonView.RPC("SlowPlayerRPC", RpcTarget.All, duration, slowValue, playerID);
    }

    [PunRPC]
    void SlowPlayerRPC(int duration, int slowValue, int playerID)
    {
        if (playerMap[playerID] != null)
        {
            playerMap[playerID].Slow(duration, slowValue);
        }
    }

    public void SlowAndDamagePlayer(int duration, int slowValue, int damage, int playerID)
    {
        photonView.RPC("SlowAndDamagePlayerRPC", RpcTarget.All, duration, slowValue, damage, playerID);
    }

    [PunRPC]
    void SlowAndDamagePlayerRPC(int duration, int slowValue, int damage, int playerID)
    {
        Player player = playerMap[playerID];

        if (player != null)
        {
            player.Slow(duration, slowValue);
            player.Damage(damage);
        }
    }

    public void DamagePlayerWithDOT(int initialDamage, int dotDamage, int dotTicks, int playerID)
    {
        photonView.RPC("DamagePlayerWithDOTRPC", RpcTarget.All, initialDamage, dotDamage, dotTicks, playerID);
    }

    [PunRPC]
    void DamagePlayerWithDOTRPC(int initialDamage, int dotDamage, int dotTicks, int playerID)
    {
        Player player = playerMap[playerID];

        if (player != null)
        {
            player.Damage(initialDamage);
            player.ApplyDOT(dotTicks, dotDamage);
        }
    }
}
