using UnityEngine;
using System.Collections;
using Photon.Pun;

public class EnvironmentManager : MonoBehaviour
{
    // This will be triggered by 1 of the abilities
    [SerializeField] GameObject dustDusk;

    SpriteRenderer dustDuskRenderer;
    Transform dustDuskTransform;

    PhotonView photonView;

    // This will help us position the walls and area limiters in the same pla
    public Vector2 environmentSize;

    public static EnvironmentManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        photonView = GetComponent<PhotonView>();

        // Let the master client decide what the environment size is based on it's screen size
        // and then update the rest of the clients with this value
        if (PhotonNetwork.IsMasterClient)
        {
            environmentSize = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
            photonView.RPC("SetEnvironmentSize", RpcTarget.OthersBuffered, environmentSize.x, environmentSize.y);
        }
    }

    private void Start()
    {
        dustDuskRenderer = dustDusk.GetComponent<SpriteRenderer>();
        dustDuskTransform = dustDusk.GetComponent<Transform>();

        ResizeToScreenSize();
        dustDusk.SetActive(false);
    }

    [PunRPC]
    void SetEnvironmentSize(float x, float y)
    {
        environmentSize = new Vector2(x, y);
    }

    public void TriggerDustDusk(int duration)
    {
        photonView.RPC("ActivateDustDusk", RpcTarget.All);

        StartCoroutine(HideDuskDust(duration));
    }

    [PunRPC]
    void ActivateDustDusk()
    {
        // We can add a fade here later
        dustDusk.SetActive(true);
    }

    IEnumerator HideDuskDust(float time)
    {
        yield return new WaitForSeconds(time);

        RemoveDusk();
    }

    // Can make fade later out of it
    void RemoveDusk()
    {
        photonView.RPC("RemoveDustDusk", RpcTarget.All);
    }

    [PunRPC]
    void RemoveDustDusk()
    {
        dustDusk.SetActive(false);
    }

    void ResizeToScreenSize()
    {

        Debug.Log("Resizing");
        transform.localScale = Vector3.one;

        float width = dustDuskRenderer.sprite.bounds.size.x;
        float height = dustDuskRenderer.sprite.bounds.size.y;

        float worldScreenHeight = Camera.main.orthographicSize * 2.0f;
        float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

        dustDuskTransform.localScale = new Vector3(worldScreenWidth / width, worldScreenHeight / height, 1);
    }
}
