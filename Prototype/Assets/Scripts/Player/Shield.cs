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
    public bool IsDamageFatal(ref int damage)
    {
        Debug.Log("Shield IsDamageFatal armor " + armor + " damage taken " + damage);
        armor -= damage;

        if (armor < 0)
        {
            spriteRenderer.enabled = false;
            return true;
        }

        // If the damage source had just enough damage to destroy the shield make sure
        // the Player.Damage() method won't apply the damage to the player
        if(armor == 0)
        {
            spriteRenderer.enabled = false;
            damage = 0;
        }
        
        return false;
    }

    public bool IsActive()
    {
        Debug.Log("Shield IsActive " + (armor <= 0));
        if (armor <= 0)
            return false;

        return true;
    }

}
