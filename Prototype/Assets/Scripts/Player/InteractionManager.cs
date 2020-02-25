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

    // Use this for initialization
    void Start()
    {
        photonView = GetComponent<PhotonView>();
        playerController = GetComponent<PlayerController>();
        player = playerController.player;
    }

    // This will be called by the root effect and then from the player because we need to disable the movement
    // Only for the player who owns the photon view, otherwise the player's controller will be disabled
    public void RootPlayer(int duration)
    {
        if (photonView.IsMine)
            player.Root(duration);
    }

}
