using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    // Default attributes
    public int health;
    public int mana;
    public int power;
    public float speed;

    // Max parameters, we use this if we want to cap some attributes
    // TODO: See if we have to implement this

    // We will use start to load the default stats for a player
    private void Start()
    {
        
    }
}
