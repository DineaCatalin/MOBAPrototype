using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilitySpawnTween : Tween
{
    public Vector3 initialScale;
    public Vector3 finalScale;
    public float duration;
    public float colliderActivationTime;

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

        if (colliderActivationTime <= 0)
            colliderActivationTime = duration;
    }

    public void setSpriteAlpha(float val)
    {
        gradientColor.a = val;
        spriteRenderer.color = gradientColor;
    }

    public override void Execute()
    {
        SetInitialData();
        Debug.Log("AbilitySpawnTween OnEnable ");
        LeanTween.scale(gameObject, finalScale, duration).setEase(scaleEaseType);
        LeanTween.value(gameObject, setSpriteAlpha, 0f, 1f, duration).setEase(fadeEaseType);
        Invoke("ActivateCollider", colliderActivationTime);
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
