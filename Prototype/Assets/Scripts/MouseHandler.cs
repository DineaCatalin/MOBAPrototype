using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseHandler : MonoBehaviour
{
    Sprite outOfRangeSprite;
    GameObject outOfRangeIndicator;

    Vector3 position;

    Coroutine deactivateCoroutine;

    public static MouseHandler Instance;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        outOfRangeSprite = AbilityDataCache.GetSpellIndicatorOutOfRangeSprite();
        
        outOfRangeIndicator = new GameObject("OutOfRangeIndicator");
        outOfRangeIndicator.transform.parent = transform;
        outOfRangeIndicator.transform.localScale = new Vector3(0.75f, 0.75f, 1);

        SpriteRenderer spRenderer = outOfRangeIndicator.AddComponent<SpriteRenderer>();
        spRenderer.sprite = outOfRangeSprite;

        outOfRangeIndicator.SetActive(false);
    }

    public void ShowOutOfRangeIndicator()
    {
        if (deactivateCoroutine != null)
            StopCoroutine(deactivateCoroutine);

        position = Utils.Instance.GetMousePosition();
        position.z = -3;

        outOfRangeIndicator.transform.position = position;
        outOfRangeIndicator.SetActive(true);

        Debug.Log("MouseHandler ShowOutOfRangeIndicator " + outOfRangeIndicator.transform.position);

        deactivateCoroutine = StartCoroutine("DeactivateIndicator");
    }

    IEnumerator DeactivateIndicator()
    {
        yield return new WaitForSeconds(0.2f);

        outOfRangeIndicator.SetActive(false);
        Debug.Log("MouseHandler DeactivateIndicator ");
    }
}
