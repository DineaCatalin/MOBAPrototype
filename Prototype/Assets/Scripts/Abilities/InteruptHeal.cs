using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteruptHeal : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag.Contains("Team"))
        {
            Player player = collision.GetComponent<Player>();

            if(player.isNetworkActive)
                player.StopHealOverTime();

            GameManager.Instance.DeactivatePlayerUIBuff(PlayerEffect.Heal, player.GetID(), Photon.Pun.RpcTarget.All);
        }
    }
}
