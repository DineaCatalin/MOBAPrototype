using UnityEngine;
using System.Collections;

public class Shield : MonoBehaviour
{
    [HideInInspector] public bool isActive;

    // How much hp this shild gives extra
    int armor;

    // Cache the sprite renderer so we can deactivate the shield graphic when the shield is 'not active'
    // Like this the shield can take of displaying the graphics instead of the player script who owns it
    //SpriteRenderer spriteRenderer;
    //AbilityScaleTween tween;

    public LocalParticleSystemManager shieldParticles;

    public GameObject shieldObject;

    private void Awake()
    {
        shieldParticles = GetComponent<LocalParticleSystemManager>();
        isActive = false;
    }

    private void Start()
    {
        //spriteRenderer = GetComponent<SpriteRenderer>();
        //tween = GetComponent<AbilityScaleTween>();
        //tween.enabled = false;
        DeactivateShieldGraphics();
    }

    public void SetArmor(int shieldBuffValue)
    {
        armor = shieldBuffValue;
        //spriteRenderer.enabled = true;
        //tween.enabled = true;
        isActive = true;
        ActivateShieldGraphics();
    }

    public void ActivateShieldGraphics()
    {
        //shieldParticles.ActivateParticleSystems();
        shieldObject.SetActive(true);
    }

    public void DeactivateShieldGraphics()
    {
        //if (isActive)
        //{
        //    //shieldParticles.DeactivateParticleSystems();
        //}
        shieldObject.SetActive(false);
    }

    //
    public void Damage(ref int damage)
    {
        Debug.Log("Shield IsDamageFatal armor " + armor + " damage taken " + damage);
        armor -= damage;

        if (armor <= 0)
        {
            DeactivateNetworkedShield();
        }
    }

    public bool IsActive()
    {
        Debug.Log("Shield IsActive " + (armor <= 0));
        //if (armor <= 0)
        //    return false;

        //return true;
        return isActive;
    }

    public void DeactivateNetworkedShield()
    {
        DeactivateLocalShield();
        EventManager.TriggerEvent(GameEvent.ShieldDestroyed);
    }

    public void DeactivateLocalShield()
    {
        armor = 0;
        isActive = false;
        //spriteRenderer.enabled = false;
        //tween.enabled = false;
        //shieldObject.SetActive(false);
        DeactivateShieldGraphics();
    }



}
