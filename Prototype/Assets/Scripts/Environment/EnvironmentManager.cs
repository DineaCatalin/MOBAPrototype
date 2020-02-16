using UnityEngine;
using System.Collections;

public class EnvironmentManager : MonoBehaviour
{
    // This will be triggered by 1 of the abilities
    [SerializeField] GameObject dustDusk;

    SpriteRenderer dustDuskRenderer;
    Transform dustDuskTransform;

    public static EnvironmentManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        dustDuskRenderer = dustDusk.GetComponent<SpriteRenderer>();
        dustDuskTransform = dustDusk.GetComponent<Transform>();

        ResizeToScreenSize();
        dustDusk.SetActive(false);
    }

    public void TriggerDustDusk(int duration)
    {
        // We can add a fade here later
        dustDusk.SetActive(true);

        StartCoroutine(HideDuskDust(duration));
    }

    IEnumerator HideDuskDust(float time)
    {
        yield return new WaitForSeconds(time);

        RemoveDusk();
    }

    // Can make fade later out of it
    void RemoveDusk()
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
