using UnityEngine;
using System.Collections;

public class Shield : MonoBehaviour
{
    // How much hp this shild gives extra
    int armor;

    // Cache the sprite renderer so we can deactivate the shield graphic when the shield is 'not active'
    // Like this the shield can take of displaying the graphics instead of the player script who owns it
    //SpriteRenderer spriteRenderer;
    //AbilityScaleTween tween;

    public GameObject shieldObject;

    private void Start()
    {
        //spriteRenderer = GetComponent<SpriteRenderer>();
        //tween = GetComponent<AbilityScaleTween>();
        //tween.enabled = false;
        shieldObject.SetActive(false);
    }

    public void SetArmor(int shieldBuffValue)
    {
        armor = shieldBuffValue;
        //spriteRenderer.enabled = true;
        //tween.enabled = true;
        shieldObject.SetActive(true);
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
        if (armor <= 0)
            return false;

        return true;
    }

    public void DeactivateNetworkedShield()
    {
        DeactivateLocalShield();
        EventManager.TriggerEvent(GameEvent.ShieldDestroyed);
    }

    public void DeactivateLocalShield()
    {
        armor = 0;
        //spriteRenderer.enabled = false;
        //tween.enabled = false;
        shieldObject.SetActive(false);
    }



}
