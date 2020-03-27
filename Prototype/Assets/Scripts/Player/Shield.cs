using UnityEngine;
using System.Collections;

public class Shield : MonoBehaviour
{
    // How much hp this shild gives extra
    int armor;

    // Cache the sprite renderer so we can deactivate the shield graphic when the shield is 'not active'
    // Like this the shield can take of displaying the graphics instead of the player script who owns it
    SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetArmor(int shieldBuffValue)
    {
        armor = shieldBuffValue;
        spriteRenderer.enabled = true;
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
        EventManager.TriggerEvent("ShieldDestroyed");
    }

    public void DeactivateLocalShield()
    {
        armor = 0;

        if(spriteRenderer != null)
            spriteRenderer.enabled = false;
    }

    

}
