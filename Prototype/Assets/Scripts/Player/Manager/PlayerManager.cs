using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    PhotonView photonView;

    Dictionary<int, Player> playerMap;

    Match match;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        photonView = GetComponent<PhotonView>();
        playerMap = new Dictionary<int, Player>();
        match = GetComponent<Match>();
    }

    public int GetPlayerCount()
    {
        return playerMap.Count;
    }

    public Dictionary<int, Player> GetPlayerMap()
    {
        return playerMap;
    }

    public void ActivateNonLocalPlayer(int playerID)
    {
        photonView.RPC("ActivateNonLocalPlayerRPC", RpcTarget.Others, playerID);
    }

    [PunRPC]
    void ActivateNonLocalPlayerRPC(int playerID)
    {
        Player player = playerMap[playerID];

        if (player != null && !player.isNetworkActive)
        {
            player.Activate();
        }
    }

    //IEnumerator ActivateNonLocalPlayerCoroutine(int playerID, float delay)
    //{
    //    yield return new WaitForSeconds(delay);

    //    Player player = playerMap[playerID];

    //    if (player != null && !player.isNetworkActive)
    //    {
    //        player.Activate();
    //    }
    //}

    public void KillNetworkedPlayer(int playerID, int killerID)
    {
        // Kill local player
        KillPlayer(playerID, killerID);

        // Make sure the rest of the clients did the same
        photonView.RPC("KillPlayerRPC", RpcTarget.Others, playerID, killerID);

        GameManager.Instance.CheckRoundEndMasterClient();
    }

    [PunRPC]
    void KillPlayerRPC(int playerID, int killerID)
    {
        KillPlayer(playerID, killerID);
    }

    void KillPlayer(int playerID, int killerID)
    {
        if (playerMap[playerID] != null)
        {
            playerMap[playerID].HandlePlayerDeath();

            if (killerID != 0)
                match.AddKillScore(killerID, playerID);

            Debug.Log("GameManager KillPlayer Player died " + playerID + " Killer " + killerID);
        }
    }

    public void AddPlayerOverNetwork(int playerID)
    {
        int teamID = GetTeamToAssign();

        AddPlayer(playerID, teamID);
        photonView.RPC("AddPlayerRPC", RpcTarget.OthersBuffered, playerID, teamID);
    }

    [PunRPC]
    void AddPlayerRPC(int playerID, int teamID)
    {
        Debug.Log("GameManager AddPlayerRPC " + playerID);

        AddPlayer(playerID, teamID);
    }

    void AddPlayer(int playerID, int teamID)
    {
        Player player = GameObject.Find("Player" + playerID).GetComponent<Player>();

        player.SetTeamSpecificData(teamID);
        playerMap.Add(playerID, player);

        match.AddMatchPlayer(player.nickName.text, playerID, teamID);
        Debug.Log("GameManager AddPlayer adding Match player " + playerID + " to team " + teamID);
    }

    int GetTeamToAssign()
    {
        int teamID = (int)PhotonNetwork.CurrentRoom.CustomProperties["spawnedPlayerTeamID"];

        if (teamID == 0)
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

    public void ActivatePlayerGraphics(int playerID)
    {
        photonView.RPC("ActivatePlayerGraphicsRPC", RpcTarget.All, playerID);
    }

    [PunRPC]
    public void ActivatePlayerGraphicsRPC(int playerID)
    {
        Debug.Log("ActivatePlayerRPC getting player " + playerID + " from map");
        playerMap[playerID].ActivateGraphics();
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

    public void RemoveNetworkedPlayer(int playerID)
    {
        RemovePlayer(playerID);
        photonView.RPC("RemovePlayerRPC", RpcTarget.Others, playerID);
    }

    [PunRPC]
    void RemovePlayerRPC(int playerID)
    {
        RemovePlayer(playerID);
    }

    void RemovePlayer(int playerID)
    {
        if (playerMap.ContainsKey(playerID))
        {
            Debug.Log("Removing from playerMap player : " + playerID);
            playerMap.Remove(playerID);
        }
    }

    //
    // -- Player Abilities Networking
    //

    public void ActivateRushAreaOverNetwork(float duration, int playerID)
    {
        ActivateRushArea(duration, playerID);
        photonView.RPC("ActivateRushAreaRPC", RpcTarget.Others, duration, playerID);
    }

    [PunRPC]
    void ActivateRushAreaRPC(float duration, int playerID)
    {
        ActivateRushArea(duration, playerID);
    }

    void ActivateRushArea(float duration, int playerID)
    {
        playerMap[playerID].ActivateRushArea(duration);
    }

    public void ActivatePlayerUIBuff(PlayerEffect buff, float duration, int playerID, RpcTarget targets)
    {
        photonView.RPC("ActivatePlayerUIBuffRPC", targets, buff, duration, playerID);
    }

    [PunRPC]
    void ActivatePlayerUIBuffRPC(PlayerEffect buff, float duration, int playerID)
    {
        playerMap[playerID].ActivateBuffUI(buff, duration);
    }

    public void DeactivatePlayerUIBuff(PlayerEffect buff, int playerID, RpcTarget targets)
    {
        photonView.RPC("DeactivatePlayerUIBuffRPC", targets, buff, playerID);
    }

    [PunRPC]
    void DeactivatePlayerUIBuffRPC(PlayerEffect buff, int playerID)
    {
        playerMap[playerID].DeactivateBuffUI(buff);
    }

    // The controller will take care of passing the message to the player to activate it's shield
    // from the ShieldAbility. It will do this because the photonView is attached to the GO the controller is attached,
    // so we can't call RPC's from the components on the child object aka the Player component
    public void ActivatePlayerShield(int armor, int playerID)
    {
        Debug.Log("GameManager ActivatePlayerShield with " + armor + " armor for player " + playerID);
        photonView.RPC("ActivatePlayerShieldRPC", RpcTarget.Others, armor, playerID);
    }

    [PunRPC]
    void ActivatePlayerShieldRPC(int armor, int playerID)
    {
        Debug.Log("GameManager ActivatePlayerShieldRPC with " + armor + " armor for player " + playerID);
        if (playerMap[playerID] != null)
        {
            playerMap[playerID].ActivateShield(armor);
        }
    }

    public void DeactivatePlayerShield(int playerID)
    {
        photonView.RPC("DeactivatePlayerShieldRPC", RpcTarget.Others, playerID);
    }

    [PunRPC]
    void DeactivatePlayerShieldRPC(int playerID)
    {
        Debug.Log("GameManager DeactivatePlayerShieldRPC for player " + playerID);
        if (playerMap[playerID] != null)
        {
            playerMap[playerID].DeactivateShield();
        }
    }
}
