using UnityEngine;
using System.Collections;

public class LocalPlayerReferences : MonoBehaviour
{
    public static GameObject playerGameObject;
    public static Player player;
    public static Transform playerTransform;
    public static Rigidbody2D playerRigidbody;
    public static Transform castOrigin;
    public static GameObject rushAreaContainer;

    public static void Load(GameObject playerGO, Player localPlayer, Transform playerTrans,
        Rigidbody2D rigidbody, Transform castOriginTransform, GameObject rushArea)
    {
        playerGameObject = playerGO;
        player = localPlayer;
        playerTransform = playerTrans;
        playerRigidbody = rigidbody;
        castOrigin = castOriginTransform;
        rushAreaContainer = rushArea;
    }
}
