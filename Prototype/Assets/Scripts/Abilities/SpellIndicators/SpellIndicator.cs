using UnityEngine;
using System.Collections;

public class SpellIndicator : MonoBehaviour
{
    [SerializeField] float alphaInCastRange;
    [SerializeField] float alphaOutOfCastRange = 25;

    public int playerID;
    public float castRange;

    Transform playerTransform;
    SpriteRenderer spriteRenderer;

    private void Start()
    {
        playerTransform = GameObject.Find("Player" + playerID).transform;
        spriteRenderer = GetComponent<SpriteRenderer>();

        Debug.Log("SpellIndicator before name is " + name);
        string spName = name.Replace("SpellIndicator(Clone)", "");
        Debug.Log("SpellIndicator after name is " + spName);

        castRange = AbilityDataCache.GetAbilityCastRange(name);

        alphaInCastRange = spriteRenderer.color.a;
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector2.Distance(playerTransform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition));

        Color color = spriteRenderer.color;

        if (distance > castRange)
            color.a = alphaOutOfCastRange;
        else
            color.a = alphaInCastRange;

        spriteRenderer.color = color;
    }
}
