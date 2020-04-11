using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilitySpawnTween : MonoBehaviour
{
    public Vector2 initialScale;
    public Vector2 finalScale;
    public float duration;
    public LeanTweenType scaleEaseType;

    public LeanTweenType fadeEaseType;

    public Collider2D abilityCollider;

    SpriteRenderer spriteRenderer;

    Color zeroAlpha;
    Color gradientColor;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>() as SpriteRenderer;
        zeroAlpha = spriteRenderer.color;
        gradientColor = spriteRenderer.color;
        zeroAlpha.a = 0;

        SetInitialData();

        if (abilityCollider == null)
            abilityCollider = GetComponent<Collider2D>();
    }

    public void setSpriteAlpha(float val)
    {
        gradientColor.a = val;
        spriteRenderer.color = gradientColor;
    }

    void OnEnable()
    {
        SetInitialData();
        Debug.Log("AbilitySpawnTween OnEnable ");
        LeanTween.scale(gameObject, finalScale, duration).setEase(scaleEaseType).setOnComplete(ActivateCollider);
        LeanTween.value(gameObject, setSpriteAlpha, 0f, 1f, duration);
    }

    void SetInitialData()
    {
        Debug.Log("AbilitySpawnTween SetInitialData localScale before " + gameObject.transform.localScale);
        gameObject.transform.localScale = initialScale;
        Debug.Log("AbilitySpawnTween SetInitialData localScale after " + gameObject.transform.localScale);

        spriteRenderer.color = zeroAlpha;
        DisableCollider();
    }

    void DisableCollider()
    {
        abilityCollider.enabled = false;
    }

    void ActivateCollider()
    {
        abilityCollider.enabled = true;
    }

}
