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
    public bool IsDamageFatal(int damage)
    {
        armor -= damage;

        if (armor <= 0)
        {
            spriteRenderer.enabled = false;
            return true;
        }
        
        return false;
    }

    public bool IsActive()
    {
        if (armor <= 0)
            return false;

        return true;
    }

}
