using UnityEngine;
using System.Collections;

public class SpellIndicator : MonoBehaviour
{
    [Range(0, 10)]
    [SerializeField] float alphaInCastRange = 5;

    [Range(0, 10)]
    [SerializeField] float alphaOutOfCastRange = 1;

    [HideInInspector] public int playerID;
    [HideInInspector] public float castRange;

    Sprite outOfRangeSprite;
    Sprite inRangeSprite;

    Transform playerTransform;
    SpriteRenderer spriteRenderer;

    private void Start()
    {
        Debug.Log("SpellIndicator for player " + playerID);

        playerTransform = GameObject.Find("Player" + playerID).transform;
        spriteRenderer = GetComponent<SpriteRenderer>();

        Debug.Log("SpellIndicator before name is " + name);
        string spName = name.Replace("SpellIndicator(Clone)", "");
        Debug.Log("SpellIndicator after name is " + spName);

        castRange = AbilityDataCache.GetAbilityCastRange(spName);
        Debug.Log("SpellIndicator castRange is " + castRange);

        alphaInCastRange = alphaInCastRange / 10;
        alphaOutOfCastRange = alphaOutOfCastRange / 10;

        inRangeSprite = spriteRenderer.sprite;
        outOfRangeSprite = AbilityDataCache.GetSpellIndicatorOutOfRangeSprite();
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector2.Distance(playerTransform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition));
        Debug.Log("SpellIndicator distance " + distance + " distance to castrange " + (distance - castRange));

        Color color = spriteRenderer.color;

        if (distance > castRange)
        {
            //color.a = alphaOutOfCastRange;
            spriteRenderer.sprite = outOfRangeSprite;
        }
        else
        {
            //color.a = alphaInCastRange;
            spriteRenderer.sprite = inRangeSprite;
        }

        spriteRenderer.color = color;
    }
}
