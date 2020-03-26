using UnityEngine;
using Photon.Pun;

public enum AbilityInputKey
{
    // Ability1 Will be left  click 
    // Ability2 Will be right click 
    AttackAbility = KeyCode.Mouse0,
    DefenseAbility = KeyCode.Mouse1,
    MobilityAbility = KeyCode.Space,
    Ability1 = KeyCode.Alpha1,
    Ability2 = KeyCode.Alpha2,
    Ability3 = KeyCode.Alpha3,
    Ability4 = KeyCode.Alpha4,
    AbilityAlias1 = KeyCode.Z,
    AbilityAlias2 = KeyCode.X,
    AbilityAlias3 = KeyCode.C,
    AbilityAlias4 = KeyCode.V,
    AbilityManaCharge = KeyCode.LeftShift    
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

    bool charging;

    // Use this for initialization
    void Start()
    {
        stats = player.GetStats();
        charging = false;

        abilityManager = player.GetComponent<AbilityManager>();
        playerTransform = player.transform;
        photonView = GetComponent<PhotonView>();
    }

    bool abilityWasCast;

    // Update is called once per frame
    void Update()
    {
        if (HandleManaCharge())
            return;

        HandleMovement();       
        HandleRotation();

        HandleAbilityCasting();
        HandleAbilitySelection();
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

    bool HandleAbilityCasting()
    {
        if(Input.GetMouseButton(0))
        {
            if (abilityManager.NoAbilitySelected() || abilityManager.IsCurrentAbilityCharging())
            {
                // Maybe show a UI text saying the current ability is still charging
                return false;
            }

            abilityManager.CastAbility();
            abilityWasCast = true;
            return true;
        }

        abilityWasCast = false;
        return false;
    }

    bool HandleManaCharge()
    {
        if(Input.GetKey((KeyCode)AbilityInputKey.AbilityManaCharge))
        {
            if(!charging)
            {
                player.StartManaCharge();
                charging = true;
            }
                
            return true;
        }
        else if(Input.GetKeyUp((KeyCode)AbilityInputKey.AbilityManaCharge))
        {
            player.StopManaCharge();
            charging = false;
        }

        return false;
    }

    void HandleAbilitySelection()
    {
        if (Input.GetMouseButton(0) && !abilityWasCast)
        {
            SwitchSelectedAbility(1);
            //return;
        }
        if (Input.GetMouseButtonDown(1))
        {
            SwitchSelectedAbility(2);
            //return;
        }
        if (Input.GetKeyDown((KeyCode)AbilityInputKey.MobilityAbility))
        {
            SwitchSelectedAbility(3);
        }
        if (Input.GetKeyDown((KeyCode)AbilityInputKey.Ability1) ||
            Input.GetKeyDown((KeyCode)AbilityInputKey.AbilityAlias1))
        {
            SwitchSelectedAbility(4);
        }
        if (Input.GetKeyDown((KeyCode)AbilityInputKey.Ability2) ||
            Input.GetKeyDown((KeyCode)AbilityInputKey.AbilityAlias2))
        {
            SwitchSelectedAbility(5);
        }
        if (Input.GetKeyDown((KeyCode)AbilityInputKey.Ability3) ||
            Input.GetKeyDown((KeyCode)AbilityInputKey.AbilityAlias3))
        {
            SwitchSelectedAbility(6);
        }
        if (Input.GetKeyDown((KeyCode)AbilityInputKey.Ability4) || 
            Input.GetKeyDown((KeyCode)AbilityInputKey.AbilityAlias4))
        {
            SwitchSelectedAbility(7);
        }
    }

    void SwitchSelectedAbility(int index)
    {
        // We do this so that the number of the ability becomes the position in the array
        index--;
        abilityManager.SetCurrentAbility(index);  
    }
}
