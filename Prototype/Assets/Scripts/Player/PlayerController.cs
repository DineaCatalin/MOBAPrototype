using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    float speed;
    AbilityManager abilityManager;

    // We will disable the movement function when this is true
    [HideInInspector] public bool isRooted;

    // This will be true when Spell indicator is shown so the player can chose where to place the ability
    bool showingSpellIndicator;


    // Use this for initialization
    void Start()
    {
        speed = GetComponent<PlayerData>().speed;
        abilityManager = GetComponent<AbilityManager>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        HandleAbilitySelection();
        HandleAbilityCasting();
    }

    void HandleMovement()
    {
        if (isRooted)
            return;

        if (Input.GetKey(KeyCode.W))       // UP
        {
            transform.Translate(new Vector3(0, 1, 0) * Time.deltaTime * speed, Space.World);
        }
        if (Input.GetKey(KeyCode.S))       // DOWN
        {
            transform.Translate(new Vector3(0, -1, 0) * Time.deltaTime * speed, Space.World);
        }
        if (Input.GetKey(KeyCode.A))       // LEFT
        {
            transform.Translate(new Vector3(-1, 0, 0) * Time.deltaTime * speed, Space.World);
        }
        if (Input.GetKey(KeyCode.D))       // RIGHT
        {
            transform.Translate(new Vector3(1, 0, 0) * Time.deltaTime * speed, Space.World);
        }
    }

    void HandleRotation()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 perpendicular = transform.position - mousePos;
        transform.rotation = Quaternion.LookRotation(Vector3.forward, perpendicular);
    }

    

    void HandleAbilityCasting()
    {
        if(Input.GetMouseButton(0))
        {
            if(abilityManager.isCurrentAbilityCharging())
            {
                // Maybe show a UI text saying the current ability is still charging
                return;
            }
            else if(abilityManager.isCurrentAbilityInstant() || showingSpellIndicator)
            {
                abilityManager.CastAbility();
                showingSpellIndicator = false;
            }

        }
    }

    void HandleAbilitySelection()
    {
        // Could initialize keys somewhere else but for now do it here
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchSelectedAbility(1);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2))
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
        if (abilityManager.isAbilitySelected(index))
            return;

        abilityManager.SetCurrentAbility(index);

        if(!abilityManager.isCurrentAbilityInstant())
        {
            showingSpellIndicator = true;
            // Enable current spell indicator
        }
            
    }


}
