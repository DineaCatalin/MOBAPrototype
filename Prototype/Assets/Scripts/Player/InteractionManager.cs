using UnityEngine;
using System.Collections;
using Photon.Pun;

// This component is made to be called by the ShieldAbility and raise an RPC
// We can't do this directly from the player or playercontroller because the 
// player is not on the GO with the PhotonView and playercontroller will be 
// deactivated  for the other players because it handles movement
public class InteractionManager : MonoBehaviour
{
    PhotonView photonView;

    Player player;

	PlayerController playerController;

    public static InteractionManager Instance;

    // Use this for initialization
    void Start()
    {
        Instance = this;

        photonView = GetComponent<PhotonView>();
		playerController = GetComponent<PlayerController>();
		player = playerController.player;
    }

    // The controller will take care of passing the message to the player to activate it's shield
    // from the ShieldAbility. It will do this because the photonView is attached to the GO the controller is attached,
    // so we can't call RPC's from the components on the child object aka the Player component
    public void ActivatePlayerShield(int armor, int playerID)
    {
        Debug.Log("InteractionManager ActivatePlayerShield with " + armor + " armor for player " + playerID);
        photonView.RPC("ActivatePlayerShieldRPC", RpcTarget.All, armor, playerID);
    }

    [PunRPC]
    void ActivatePlayerShieldRPC(int armor, int playerID)
    {
        Debug.Log("InteractionManager ActivatePlayerShieldRPC with " + armor + " armor for player " + playerID);
        player.ActivateShield(armor, playerID);
    }

	public void KnockOutPlayer(int force, int damage)
	{
        // Do this only for our player, the other representations of this player will be synched by PUN2_PlayerSync
		if(photonView.IsMine)
		{
			player.Knockout(force, damage);
		}
	}
}
