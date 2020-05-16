using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerVitals : MonoBehaviour
{
    //int hitPoints
    //{
    //    get
    //    {
    //        object hp;
    //        if (photonView.owner.CustomProperties.TryGetValue("HP", out hp))
    //            return (int)hp;
    //        else
    //            return MAX_HP;
    //    }
    //    set
    //    {
    //        player.SetCustomProperty("HP", value);
    //    }
    //}

    PlayerData data;
    PhotonView photonView;

    // Start is called before the first frame update
    void Awake()
    {
        data = GetComponent<Player>().stats;
        photonView = GetComponent<PhotonView>();
    }

    
}
