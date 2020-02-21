﻿using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    // This is where the abilitymanager, transform and player scripts are
    [SerializeField] GameObject player;

    AbilityManager abilityManager;
    Transform playerTransform;
  

    // We will disable the movement function when this is true
    public bool isRooted;

    // We will use the arrows for moving dummy players
    // Used only for testing 
    [SerializeField] bool isDummy;
    
    PlayerData stats;

    Vector3 movementIncrement;

    // Use this for initialization
    void Start()
    {
        stats = player.GetComponent<Player>().GetStats();
        abilityManager = player.GetComponent<AbilityManager>();
        playerTransform = player.transform;
    }

    // Update is called once per frame
    void Update()
    {
        // Delete this later
        if(isDummy)
        {
            HandleDummyMovement();
            return;
        }

        HandleMovement();       
        HandleRotation();
        HandleAbilitySelection();
        HandleAbilityCasting();
    }

    void HandleMovement()
    {
        if (isRooted)
            return;

        movementIncrement = Vector3.zero;

        if (Input.GetKey(KeyCode.W))       // UP
        {
            movementIncrement += Vector3.up;
        }
        if (Input.GetKey(KeyCode.S))       // DOWN
        {
            movementIncrement += Vector3.down;
        }
        if (Input.GetKey(KeyCode.A))       // LEFT
        {
            movementIncrement += Vector3.left;
        }
        if (Input.GetKey(KeyCode.D))       // RIGHT
        {
            movementIncrement += Vector3.right;
        }

        playerTransform.Translate(movementIncrement * Time.deltaTime * stats.speed, Space.World);
    }

    void HandleRotation()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 perpendicular = playerTransform.position - mousePos;
        playerTransform.rotation = Quaternion.LookRotation(Vector3.forward, perpendicular);
    }

    void HandleAbilityCasting()
    {
        if(Input.GetMouseButton(0))
        {
            if(abilityManager.NoAbilitySelected() || abilityManager.IsCurrentAbilityCharging())
            {
                // Maybe show a UI text saying the current ability is still charging
                return;
            }

            abilityManager.CastAbility();
        }
    }

    void HandleAbilitySelection()
    {
        // Could initialize keys somewhere else but for now do it here
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchSelectedAbility(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchSelectedAbility(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SwitchSelectedAbility(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SwitchSelectedAbility(4);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SwitchSelectedAbility(5);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            SwitchSelectedAbility(6);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            SwitchSelectedAbility(7);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            SwitchSelectedAbility(8);
        }

    }

    void SwitchSelectedAbility(int index)
    {
        // We do this so that the number of the ability becomes the position in the array
        index--;

        abilityManager.SetCurrentAbility(index);  
    }

    void HandleDummyMovement()
    {
        if (isRooted)
            return;

        movementIncrement = Vector3.zero;

        if (Input.GetKey(KeyCode.UpArrow))       // UP
        {
            movementIncrement += Vector3.up;
        }
        if (Input.GetKey(KeyCode.DownArrow))       // DOWN
        {
            movementIncrement += Vector3.down;
        }
        if (Input.GetKey(KeyCode.LeftArrow))       // LEFT
        {
            movementIncrement += Vector3.left;
        }
        if (Input.GetKey(KeyCode.RightArrow))       // RIGHT
        {
            movementIncrement += Vector3.right;
        }

        playerTransform.Translate(movementIncrement * Time.deltaTime * stats.speed, Space.World);
    }

}
