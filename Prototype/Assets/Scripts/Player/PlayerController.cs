using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    AbilityManager abilityManager;

    // We will disable the movement function when this is true
    [HideInInspector] public bool isRooted;

    PlayerData stats;

    // Use this for initialization
    void Start()
    {
        stats = GetComponent<Player>().GetStats();
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
            transform.Translate(new Vector3(0, 1, 0) * Time.deltaTime * stats.speed, Space.World);
        }
        if (Input.GetKey(KeyCode.S))       // DOWN
        {
            transform.Translate(new Vector3(0, -1, 0) * Time.deltaTime * stats.speed, Space.World);
        }
        if (Input.GetKey(KeyCode.A))       // LEFT
        {
            transform.Translate(new Vector3(-1, 0, 0) * Time.deltaTime * stats.speed, Space.World);
        }
        if (Input.GetKey(KeyCode.D))       // RIGHT
        {
            transform.Translate(new Vector3(1, 0, 0) * Time.deltaTime * stats.speed, Space.World);
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
        //else if (Input.GetKeyDown(KeyCode.Alpha3))
        //{
        //    SwitchSelectedAbility(3);
        //}
        //else if (Input.GetKeyDown(KeyCode.Alpha4))
        //{
        //    SwitchSelectedAbility(4);
        //}
        //else if (Input.GetKeyDown(KeyCode.Alpha5))
        //{
        //    SwitchSelectedAbility(5);
        //}
        //else if (Input.GetKeyDown(KeyCode.Alpha6))
        //{
        //    SwitchSelectedAbility(6);
        //}
        //else if (Input.GetKeyDown(KeyCode.Alpha7))
        //{
        //    SwitchSelectedAbility(7);
        //}
        //else if (Input.GetKeyDown(KeyCode.Alpha8))
        //{
        //    SwitchSelectedAbility(8);
        //}

    }

    void SwitchSelectedAbility(int index)
    {
        // We do this so that the number of the ability becomes the position in the array
        index--;

        abilityManager.SetCurrentAbility(index);  
    }


}
