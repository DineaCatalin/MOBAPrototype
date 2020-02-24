using UnityEngine;
using System.Collections;

public class Lava : MonoBehaviour
{
    [SerializeField] int damage;

    // Force with which the player will be thrown away
    [SerializeField] int pushBackForce;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // We hit a player
        if (collision.tag.Contains("Team"))
        {
            Player player = collision.GetComponent<Player>();
            player.Knockout(pushBackForce, damage);
            GameManager.Instance.KnockOutPlayer(pushBackForce, damage, player.GetID());
        }
    }
}
