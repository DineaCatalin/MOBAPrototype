using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class TestProjectile : MonoBehaviour
{
    [SerializeField] GameObject testAbility;

    PhotonView photonView;

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
    }

    [PunRPC]
    private void Spawn()
    {
        Instantiate(testAbility, Vector3.zero, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            photonView.RPC("Spawn", RpcTarget.All);
        }
    }
}
