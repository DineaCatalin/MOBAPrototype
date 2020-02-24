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


}
