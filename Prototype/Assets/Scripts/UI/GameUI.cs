using UnityEngine;
using System.Collections;

// This will contain most of the UI elements in the game
// Except the player health and mana bars
// So cooldowns, match stats etc...
public class GameUI : MonoBehaviour
{
    public static GameUI Instance;

    // Use this for initialization
    void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
