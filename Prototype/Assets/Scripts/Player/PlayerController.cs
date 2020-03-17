using UnityEngine;
using System.Collections;
using Photon.Pun;

public enum AbilityInputKey
{
    Ability1 = KeyCode.Alpha1,
    Ability2 = KeyCode.Alpha2,
    Ability3 = KeyCode.Alpha3,
    Ability4 = KeyCode.Alpha4,
    Ability5 = KeyCode.R,
    Ability6 = KeyCode.T,
    Ability7 = KeyCode.F,
    Ability8 = KeyCode.C
}

public class PlayerController : MonoBehaviour
{
    public Player player;

    AbilityManager abilityManager;
    Transform playerTransform;

    // We will disable the movement function when this is true
    public bool isRooted;
    
    PlayerData stats;

    Vector3 movementIncrement;

    PhotonView photonView;

    // Use this for initialization
    void Start()
    {
        //player = playerGO.GetComponent<Player>();
        stats = player.GetStats();

        abilityManager = player.GetComponent<AbilityManager>();
        playerTransform = player.transform;
        photonView = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
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
            if (abilityManager.NoAbilitySelected() || abilityManager.IsCurrentAbilityCharging())
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
        if(Input.GetKeyDown((KeyCode)AbilityInputKey.Ability1))
        {
            SwitchSelectedAbility(1);
        }
        else if (Input.GetKeyDown((KeyCode)AbilityInputKey.Ability2))
        {
            SwitchSelectedAbility(2);
        }
        else if (Input.GetKeyDown((KeyCode)AbilityInputKey.Ability3))
        {
            SwitchSelectedAbility(3);
        }
        else if (Input.GetKeyDown((KeyCode)AbilityInputKey.Ability4))
        {
            SwitchSelectedAbility(4);
        }
        else if (Input.GetKeyDown((KeyCode)AbilityInputKey.Ability5))
        {
            SwitchSelectedAbility(5);
        }
        else if (Input.GetKeyDown((KeyCode)AbilityInputKey.Ability6))
        {
            SwitchSelectedAbility(6);
        }
        else if (Input.GetKeyDown((KeyCode)AbilityInputKey.Ability7))
        {
            SwitchSelectedAbility(7);
        }
        else if (Input.GetKeyDown((KeyCode)AbilityInputKey.Ability8))
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
}
