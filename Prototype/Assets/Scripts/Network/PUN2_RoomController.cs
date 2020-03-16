using UnityEngine;
using System;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PUN2_RoomController : MonoBehaviourPunCallbacks
{
    //Player instance prefab, must be located in the Resources folder
    public GameObject playerPrefab;

    //Player spawn point
    public Vector3 spawnPoint;

    // This will be set when the player is spawned and will be used to remove the player from the 
    int localPlayerID;

    // Use this for initialization
    void Start()
    {
        EventManager.StartListening("DraftFinished", new Action(OnDraftFinished));

        //In case we started this demo with the wrong scene being active, simply load the menu scene
        if (PhotonNetwork.CurrentRoom == null)
        {
            Debug.Log("Is not in the room, returning back to Lobby");
            UnityEngine.SceneManagement.SceneManager.LoadScene("GameLobby");
            return;
        }

        // Assign room properties from the master client
        // Current team ID is used to know
        if (PhotonNetwork.IsMasterClient)
        {
            Hashtable roomProperties = new Hashtable();
            roomProperties.Add("spawnedPlayerTeamID", 0);

            for (int i = 0; i < GameManager.MAX_PLAYERS; i++)
            {
                roomProperties.Add("ID_Player_" + i.ToString(), 0);
            }

            PhotonNetwork.CurrentRoom.SetCustomProperties(roomProperties);
        }

        if (spawnPoint == Vector3.zero)
        {
            spawnPoint = Utils.GetRandomScreenPoint();
        }
    }

    void OnDraftFinished()
    {
        Debug.Log("PUN2_RoomController spawning player");

        //We're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
        var playerGO = PhotonNetwork.Instantiate(playerPrefab.name, spawnPoint, Quaternion.identity, 0);

        // Get the player script on the child object of the player prefab
        // The NetworkedPlayer contains the PlayController and has the player GO as a child. This playerGO has
        // the player, abilitymanager etc scripts on it so to get the player script we do the following "chain"
        Player player = playerGO.GetComponent<PlayerController>().player.GetComponent<Player>();

        // Cache the ID of our local player
        if (photonView.IsMine)
            localPlayerID = player.GetID();

        Debug.Log("PUN2_RoomController Instantiating player " + player.GetID());

        // Now we add the player to the GameMangers Player Container
        GameManager.Instance.AddPlayer(player.GetID());
    }

    void OnGUI()
    {
        if (PhotonNetwork.CurrentRoom == null)
            return;

        //Leave this Room
        if (GUI.Button(new Rect(5, 5, 125, 25), "Leave Room"))
        {
            PhotonNetwork.LeaveRoom();
        }

        //Show the Room name
        GUI.Label(new Rect(135, 5, 200, 25), PhotonNetwork.CurrentRoom.Name);

        //Show the list of the players connected to this Room
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            //Show if this player is a Master Client. There can only be one Master Client per Room so use this to define the authoritative logic etc.)
            string isMasterClient = (PhotonNetwork.PlayerList[i].IsMasterClient ? ": MasterClient" : "");
            GUI.Label(new Rect(5, 35 + 30 * i, 200, 25), PhotonNetwork.PlayerList[i].NickName + isMasterClient);
        }
    }

    public override void OnLeftRoom()
    {
        Debug.Log("PUN2_RoomController OnLeftRoom removing from playerMap player " + localPlayerID);

        // Remove the player from the GameManager playerMap
        GameManager.Instance.RemovePlayer(localPlayerID);

        //We have left the Room, return back to the GameLobby
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameLobby");
    }
}
