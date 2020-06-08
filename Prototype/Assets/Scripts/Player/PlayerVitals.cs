using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerVitals : MonoBehaviour
{
    PhotonView photonView;
    PlayerData stats;

    public StatsBar healthBar;
    public StatsBar manaBar;

    // Start is called before the first frame update
    void Awake()
    {
        stats = GetComponentInChildren<Player>().stats;
        photonView = GetComponentInParent<PhotonView>();
    }

    public void SetHealthNetworked(int hp)
    {
        SetHealth(hp);
        photonView.RPC("SetHealthRPC", RpcTarget.Others, hp);
    }

    public void SetManaNetworked(int mana)
    {
        SetMana(mana);
        photonView.RPC("SetManaRPC", RpcTarget.Others, mana);
    }

    
    public void SetHealth(int hp)
    {
        Debug.Log("DHC PlayerVitals SetHealth stats.health " + stats.health + " is set to " + hp);
        //stats.health = hp;
        healthBar.SetCurrentStat(hp);
    }

    public void SetMana(int mana)
    {
        //stats.mana = mana;
        manaBar.SetCurrentStat(mana);
    }

    [PunRPC]
    void SetHealthRPC(int hp)
    {
        SetHealth(hp);
    }

    [PunRPC]
    public void SetManaRPC(int mana)
    {
        SetMana(mana);
    }

}
