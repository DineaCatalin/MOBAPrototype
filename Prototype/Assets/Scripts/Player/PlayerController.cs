using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    float speed;
    AbilityManager abilityManager;

    // We will disable the movement function when this is true
    [HideInInspector] public bool isRooted; 

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
        HandleAbilities();
        Test();
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

    // This will be true when Spell indicator is shown so the player can chose where to place the ability
    bool showingSpellIndicator;

    void Test()
    {
        if(Input.GetMouseButton(0))
        {
            if(abilityManager.isCurrentAbilityCharging())
            {
                // Maybe show a UI text saying the current ability is still charging
                return;
            }
            else if(abilityManager.isCurrentAbilityInstant())
            {
                abilityManager.CastAbility();
            }
            else if(showingSpellIndicator)
            {
                abilityManager.CastAbility();
                showingSpellIndicator = false;
            }

        }
    }

    // Use this to know 
    int currentAbilityIndex;

    void HandleAbilities()
    {
        // Could initialize keys somewhere else but for now do it here
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentAbilityIndex = 1;
            showingSpellIndicator = true;
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentAbilityIndex = 2;
            showingSpellIndicator = true;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            currentAbilityIndex = 3;
            showingSpellIndicator = true;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            currentAbilityIndex = 4;
            showingSpellIndicator = true;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            currentAbilityIndex = 5;
            showingSpellIndicator = true;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            currentAbilityIndex = 6;
            showingSpellIndicator = true;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            currentAbilityIndex = 7;
            showingSpellIndicator = true;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            currentAbilityIndex = 8;
            showingSpellIndicator = true;
        }

    }


}
